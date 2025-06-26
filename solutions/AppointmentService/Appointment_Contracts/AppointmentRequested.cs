namespace Appointment_Contracts;

public sealed record AppointmentRequested()
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid SlotId { get; set; }

    public AppointmentStatus Status { get; set; }
     
};
 
public enum AppointmentStatus
{
    CONFIRMED,
    CANCELLED,
    PENDING
}