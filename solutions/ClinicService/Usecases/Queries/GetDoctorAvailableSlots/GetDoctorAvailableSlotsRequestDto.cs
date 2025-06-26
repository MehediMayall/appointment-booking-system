namespace ClinicService;


public sealed record GetDoctorAvailableSlotsRequestDto(string Specialization, Guid? ClinicId);