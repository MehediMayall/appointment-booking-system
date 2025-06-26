using Appointment_Contracts;

namespace AppointmentService;

public record AppointmentRequestCommand(AppointmentRequestRequestDto requestDto) : IRequest<Response<AppointmentRequestResponseDto>>{}
public sealed class AppointmentRequestCommandHandler(
    IHybridCacheService _hybridCache,
    IAppointmentRequestRepository _repo,
    MassTransit.IPublishEndpoint _publish
    ) : IRequestHandler<AppointmentRequestCommand, Response<AppointmentRequestResponseDto>>
{


    // Step1: Check if Appointment already exists
    // Step2: if exists return success
    // Step3: if not,  create new Appointment
    // Step4: Save New Appointment into Cache
    // Step5: Send AppointmentRequested Event to Notification Service
    // Step6: return success
    public async Task<Response<AppointmentRequestResponseDto>> Handle(AppointmentRequestCommand request, CancellationToken cancellationToken)
    {
        // Check if Appointment already exists
        Appointment existingAppointment = await _repo.Get(
            t => t.DoctorId == request.requestDto.DoctorId &&
            t.SlotId == request.requestDto.SlotId &&
            t.PatientId == request.requestDto.PatientId &&
            t.IsActive == true
        );

        // if exists return success
        if (existingAppointment is not null)
            return new AppointmentRequestResponseDto("Success");

        // Create New Appointment
        var newAppointment = request.requestDto.New();

        // Save New Appointment into Cache
        await _hybridCache.SetAsync(
            RedisKeys.GetNewAppointmentKey(newAppointment.Id),
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
        return new AppointmentRequestResponseDto("Success");
    }



}



