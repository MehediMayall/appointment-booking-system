namespace ClinicService;

public sealed record SearchAvailableSlotsResponseDto(IReadOnlyCollection<AvailableSlotsDto> Slots);