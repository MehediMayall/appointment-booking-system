namespace ClinicService;


public sealed record SearchAvailableSlotsRequestDto()
{
    public string SearchText { get; init; }
};