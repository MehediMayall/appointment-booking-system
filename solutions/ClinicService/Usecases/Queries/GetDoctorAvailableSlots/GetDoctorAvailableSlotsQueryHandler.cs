namespace ClinicService;

public record GetDoctorAvailableSlotsQuery(GetDoctorAvailableSlotsRequestDto requestDto) : IRequest<Response<GetDoctorAvailableSlotsResponseDto>>{}
public sealed class GetDoctorAvailableSlotsQueryHandler(IGetDoctorAvailableSlotsRepository _repo) : IRequestHandler<GetDoctorAvailableSlotsQuery, Response<GetDoctorAvailableSlotsResponseDto>>
{
 


    // Step 1: Load specific clinic  and specialization slots
    // Step 2: Return GetDoctorAvailableSlots Response
    public async Task<Response<GetDoctorAvailableSlotsResponseDto>> Handle(GetDoctorAvailableSlotsQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Load specific clinic  and specialization slots
        var availableSlots = await _repo.GetDoctorAvailableSlots(
            request.requestDto.ClinicId,
            request.requestDto.Specialization
        );

        // Step 3: Return GetDoctorAvailableSlots Response
        return new GetDoctorAvailableSlotsResponseDto(availableSlots.ToList());
    }





}