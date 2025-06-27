namespace ClinicService;

public record GetAvailableSlotsQuery() : IRequest<Response<GetAvailableSlotsResponseDto>>{}
public sealed class GetAvailableSlotsQueryHandler(IGetDoctorAvailableSlotsRepository _repo) : IRequestHandler<GetAvailableSlotsQuery, Response<GetAvailableSlotsResponseDto>>
{
 


    // Step 1: Load specific clinic  and specialization slots
    // Step 2: Return GetAvailableSlots Response
    public async Task<Response<GetAvailableSlotsResponseDto>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Load specific clinic  and specialization slots
        var availableSlots = await _repo.GetAvailableSlots();

        // Step 2: Return GetAvailableSlots Response
        return new GetAvailableSlotsResponseDto(availableSlots.ToList());
    }





}