namespace ClinicService;


public class DoctorAvailableSlotsDto
{
    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public Guid DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
