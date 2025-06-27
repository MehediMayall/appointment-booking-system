using Appointment_Contracts;

namespace AppointmentService;

public record AppointmentConfirmCommand(AppointmentConfirmRequestDto requestDto) : IRequest<Response<AppointmentConfirmResponseDto>>{}
public sealed class AppointmentConfirmCommandHandler(
    IHybridCacheService _hybridCache,
    IAppointmentConfirmRepository _repo,
    MassTransit.IPublishEndpoint _publish,
    IUnitOfWork _unitOfWork,
    HttpClient _httpClient
    ) : IRequestHandler<AppointmentConfirmCommand, Response<AppointmentConfirmResponseDto>>
{


    // Step1: Check if Appointment already exists
    // Step2: if exists return success
    // Step3: if not,  create new Appointment
    // Step4: Save New Appointment into Cache
    // Step5: Send AppointmentConfirmed Event to Notification Service
    // Step6: return success
    public async Task<Response<AppointmentConfirmResponseDto>> Handle(AppointmentConfirmCommand request, CancellationToken cancellationToken)
    {
        // Check if Appointment already exists
        var pendingAppointmentResult = await _hybridCache.GetOrCreateAsync<Appointment>(
            RedisKeys.GetNewAppointmentKey(request.requestDto.AppointmentId),
            async entry => null
        );


        // If Appointment does not exist
        if (pendingAppointmentResult.IsFailure)
            return new AppointmentConfirmResponseDto("Failed");

        Appointment appointment = pendingAppointmentResult.Value;
        if (appointment is null)
            return new AppointmentConfirmResponseDto("Failed");



        // Check if requested slot is available
        var isAvailableSlots = await IsSpecificSlotAvailable(appointment.SlotId);
 
        if (isAvailableSlots is false)
            return Error.New("Slot is not available");

        // Set appointment to confirmed and save
        appointment.Status = AppointmentStatus.CONFIRMED;
        await _repo.Add(appointment);


        // Commit
        var commitResult = await _unitOfWork.SaveChangesAsync();
        if (commitResult.IsFailure)
            return commitResult.Error;


        // Send AppointmentConfirmed Event to Notification Service
        await _publish.Publish(
            new AppointmentBooked()
            {
                AppointmentId = appointment.Id,
                SlotId = appointment.SlotId
            }
        );

        // Return Success
        return new AppointmentConfirmResponseDto("Success");
    }
    
    private async Task<bool> IsSpecificSlotAvailable(Guid SlotId) 
    {
        try 
        {


            _httpClient.DefaultRequestHeaders.Add("DeviceType", "web");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    
            var response = await _httpClient.GetAsync($"http://localhost:3200/slot/available/{SlotId}");

            // Check if the request was successful
            if (!response.IsSuccessStatusCode)
                return true;

            // Read and return the response content
            var responseContent = await response.Content.ReadFromJsonAsync<HttpClientResponseDto>();
            return responseContent.Data.IsAvailable;

        }
        catch (Exception ex) {
            string errorMessage = ex.GetAllExceptions();
            Log.Error($"Error in GetAllAvailableSlots: {errorMessage}");
            return true;
        }
    }



}



