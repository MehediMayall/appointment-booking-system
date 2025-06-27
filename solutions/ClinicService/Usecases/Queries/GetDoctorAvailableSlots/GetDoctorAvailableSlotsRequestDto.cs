namespace ClinicService;


public sealed record GetDoctorAvailableSlotsRequestDto()
{
    public Guid ClinicId { get; init; }
    public string Specialization { get; init; }
};