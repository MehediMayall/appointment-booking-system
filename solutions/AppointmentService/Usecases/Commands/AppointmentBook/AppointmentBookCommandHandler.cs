namespace AppointmentService;

public record AppointmentBookCommand(AppointmentBookRequestDto requestDto) : IRequest<Response<AppointmentBookResponseDto>>{}
public sealed class AppointmentBookCommandHandler(IHybridCacheService _hybridCache, AppointmentBookRepository _repo) : IRequestHandler<AppointmentBookCommand, Response<AppointmentBookResponseDto>>
{
 

    // Step1: Check if Appointment already exists
    // Step2: if exists return success
    // Step3: if not, Save new Appointment
    // Step4: return success
    public async Task<Response<AppointmentBookResponseDto>> Handle(AppointmentBookCommand request, CancellationToken cancellationToken)
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
            return new AppointmentBookResponseDto("Success");

        var newAppointment = request.requestDto.New();

        await _hybridCache.SetAsync(
            RedisKeys.GetNewAppointmentKey(newAppointment.Id),
            newAppointment
        );

        // Return Success
        return new AppointmentBookResponseDto("Success");
    }



}



