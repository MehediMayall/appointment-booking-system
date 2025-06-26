namespace ClinicService;

public class Slot : EntityBase<Guid>
{

    public Guid ClinicId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsBooked { get; set; } = false;
    public Doctor doctor { get; set; }
    public Clinic clinic { get; set; }
}
