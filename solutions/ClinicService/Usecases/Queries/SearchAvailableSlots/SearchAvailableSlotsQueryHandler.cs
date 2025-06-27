namespace ClinicService;

public record SearchAvailableSlotsQuery(SearchAvailableSlotsRequestDto requestDto) : IRequest<Response<SearchAvailableSlotsResponseDto>>{}
public sealed class SearchAvailableSlotsQueryHandler(IHybridCacheService _hybridCache, IGetDoctorAvailableSlotsRepository _repo) : IRequestHandler<SearchAvailableSlotsQuery, Response<SearchAvailableSlotsResponseDto>>
{



    // Step 1: Load all available slots from hybrid cache    
    // Step 2: Search available slots by specialization, first name, last name, clinic name
    // Step 3: Return AvailableSlots Response
    public async Task<Response<SearchAvailableSlotsResponseDto>> Handle(SearchAvailableSlotsQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Load all available slots from hybrid cache
        var availableSlotsResult = await _hybridCache.GetOrCreateAsync<IEnumerable<AvailableSlotsDto>>(
            RedisKeys.GetAvailableSlotsKey(),
            async entry => await _repo.GetAvailableSlots()
        );


        if (availableSlotsResult.IsFailure || (availableSlotsResult.IsSuccess && availableSlotsResult.Value is null))
            return SearchAvailableSlotsErrors.NotFound();


        IEnumerable<AvailableSlotsDto> availableSlots = availableSlotsResult.Value;
        string searchText = request.requestDto.SearchText.ToLower();

        // Search available slots by specialization, first name, last name, clinic name
        IEnumerable<AvailableSlotsDto> searchedAvailableSlots =
            availableSlots.Where(
                t => t.Specialization.ToLower().Contains(searchText) ||   // Search by specialization
                t.FirstName.ToLower().Contains(searchText) ||     // Search by first name
                t.LastName.ToLower().Contains(searchText) ||      // Search by last name
                t.ClinicName.ToLower().Contains(searchText)       // Search by clinic name
            ).ToList();


        // Step 3: Return AvailableSlots Response
        return new SearchAvailableSlotsResponseDto(searchedAvailableSlots.ToList());
    }





}