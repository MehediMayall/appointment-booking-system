using Appointment_Contracts;

namespace AppointmentService;

public record AppointmentConfirmCommand(AppointmentConfirmRequestDto requestDto) : IRequest<Response<AppointmentConfirmResponseDto>>{}
public sealed class AppointmentConfirmCommandHandler(
    IHybridCacheService _hybridCache,
    IAppointmentConfirmRepository _repo,
    MassTransit.IPublishEndpoint _publish,
    IUnitOfWork _unitOfWork
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




        // // if exists return success
        // if (existingAppointment is not null)
        //     return new AppointmentConfirmResponseDto("Success");

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
                AppointmentId = appointment.Id
            }
        );

        // Return Success
        return new AppointmentConfirmResponseDto("Success");
    }



}



