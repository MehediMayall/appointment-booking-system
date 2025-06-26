namespace AppointmentService;

public class Appointment : EntityBase<Guid>
{

    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid SlotId { get; set; }
    public AppointmentStatus Status { get; set; }
}
