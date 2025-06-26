namespace Appointment_Contracts;

public sealed record AppointmentBooked()
{
    public Guid AppointmentId { get; set; }
     
};
