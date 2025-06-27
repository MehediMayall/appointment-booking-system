namespace ClinicService;

public sealed record GetAvailableSlotsResponseDto(IReadOnlyCollection<AvailableSlotsDto> Slots);