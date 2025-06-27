
namespace AppointmentService;

public sealed record AppointmentConfirmResponseDto(string Message);


public sealed record HttpClientResponseDto
{
    public SpecificSlot Data { get; init; }
    public int StatusCode { get; init; }
    public string Message { get; init; }
}
public sealed record SpecificSlot
{
    public bool IsAvailable { get; init; }
}