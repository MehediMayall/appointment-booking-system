namespace ClinicService;

public sealed class AvailableSlotsDto : DoctorAvailableSlotsDto
{
    public string ClinicName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    
}