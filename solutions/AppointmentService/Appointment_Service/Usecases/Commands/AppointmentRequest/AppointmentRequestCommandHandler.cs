using Appointment_Contracts;

namespace AppointmentService;

public record AppointmentRequestCommand(AppointmentRequestRequestDto requestDto) : IRequest<Response<AppointmentRequestResponseDto>>{}
public sealed class AppointmentRequestCommandHandler(
    IHybridCacheService _hybridCache,
    IAppointmentRequestRepository _repo,
    MassTransit.IPublishEndpoint _publish,
    HttpClient _httpClient
    ) : IRequestHandler<AppointmentRequestCommand, Response<AppointmentRequestResponseDto>>
{


    // Step1: Check if Appointment already exists
    // Step2: if exists return success
    // Step3: if not,  create new Appointment
    // Step4: Load all available slots from hybrid cache
    // Step4: Check if requested slot is available
    
    // Step5: Save New Appointment into Cache
    // Step6: Send AppointmentRequested Event to Notification Service
    // Step7: return success
    public async Task<Response<AppointmentRequestResponseDto>> Handle(AppointmentRequestCommand request, CancellationToken cancellationToken)
    {
        // Check if Appointment already exists
        var recentlyAppointedResult = await _hybridCache.GetOrCreateAsync<Appointment>(
            RedisKeys.GetNewAppointmentKey(request.requestDto.SlotId),
            async entry => null
        );

        Appointment existingAppointment = null;
        if (recentlyAppointedResult.IsSuccess)
            existingAppointment = recentlyAppointedResult.Value;

        // if exists return success
        if (existingAppointment is not null)
            return Error.New("Failed - Appointment already exists");


        // Load all available slots from hybrid cache
        var availableSlotsResult = await _hybridCache.GetOrCreateAsync<IEnumerable<AvailableSlotsDto>>(
            RedisKeys.GetAvailableSlotsKey(),
            async entry => await GetAllAvailableSlots()
        );

        // Check if there are available slots
        if (availableSlotsResult.IsFailure || (availableSlotsResult.IsSuccess && availableSlotsResult.Value is null))
            return Error.New("Failed - No available slots");


        // Check if requested slot is available
        IEnumerable<AvailableSlotsDto> availableSlots = availableSlotsResult.Value;
        var requestedSlot = availableSlots.FirstOrDefault(x => x.Id == request.requestDto.SlotId);
        if (requestedSlot is null)
            return Error.New("Slot is not available");


        // Create New Appointment
        var newAppointment = request.requestDto.New();

        // Save New Appointment into Cache
        await _hybridCache.SetAsync(
            RedisKeys.GetNewAppointmentKey(newAppointment.Id),
            newAppointment
        );


        // Save As Recently Appointed into Cache
        await _hybridCache.SetAsync(
            RedisKeys.GetRecentlyAppointedKey(newAppointment.SlotId),
            newAppointment
        );


        // Send AppointmentRequested Event to Notification Service
        await _publish.Publish(
            new AppointmentRequested()
            {
                Id = newAppointment.Id,
                DoctorId = newAppointment.DoctorId,
                PatientId = newAppointment.PatientId,
                SlotId = newAppointment.SlotId,
                Status = Appointment_Contracts.AppointmentStatus.PENDING
            }
        );

        // Return Success
        return new AppointmentRequestResponseDto(newAppointment.Id);
    }
    
    private async Task<IEnumerable<AvailableSlotsDto>> GetAllAvailableSlots() 
    {
        try 
        {

            _httpClient.DefaultRequestHeaders.Add("DeviceType", "web");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    
            var response = await _httpClient.GetAsync("http://localhost:3200/available/slots");

            // Check if the request was successful
            if (!response.IsSuccessStatusCode)
                return null;

            // Read and return the response content
            var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<AvailableSlotsDto>>();
            return responseContent;

        }
        catch (Exception ex) {
            string errorMessage = ex.GetAllExceptions();
            Log.Error($"Error in GetAllAvailableSlots: {errorMessage}");
            return null;
        }
    }



}



