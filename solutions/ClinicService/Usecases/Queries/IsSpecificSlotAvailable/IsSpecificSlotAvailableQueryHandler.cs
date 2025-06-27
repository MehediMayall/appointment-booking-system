namespace ClinicService;

public record IsSpecificSlotAvailableQuery(Guid SlotId) : IRequest<Response<IsSpecificSlotAvailableResponseDto>>{}
public sealed class IsSpecificSlotAvailableQueryHandler(IGetDoctorAvailableSlotsRepository _repo) : IRequestHandler<IsSpecificSlotAvailableQuery, Response<IsSpecificSlotAvailableResponseDto>>
{
 


    // Step 1: Load specific clinic  and specialization slots
    // Step 2: Return IsSpecificSlotAvailable Response
    public async Task<Response<IsSpecificSlotAvailableResponseDto>> Handle(IsSpecificSlotAvailableQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Load specific clinic  and specialization slots
        var isAvailable = await _repo.IsSpecificSlotAvailable(request.SlotId);

        // Step 2: Return IsSpecificSlotAvailable Response
        return new IsSpecificSlotAvailableResponseDto(isAvailable);
    }





}