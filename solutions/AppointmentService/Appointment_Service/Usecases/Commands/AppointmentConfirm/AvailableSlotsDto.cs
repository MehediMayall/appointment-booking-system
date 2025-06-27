namespace AppointmentService;

public sealed class AvailableSlotsDto
{


    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public string ClinicName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public Guid DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
}