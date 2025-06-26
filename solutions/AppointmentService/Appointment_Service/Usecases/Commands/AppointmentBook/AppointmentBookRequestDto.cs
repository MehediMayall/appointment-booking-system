using Appointment_Contracts;

namespace AppointmentService;

public sealed record AppointmentBookRequestDto()
{
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid SlotId { get; set; }

    public Appointment New()
    {
        return new Appointment()
        {
            Id = Guid.NewGuid(),
            DoctorId = DoctorId,
            PatientId = PatientId,
            SlotId = SlotId,
            Status = AppointmentStatus.PENDING
        };
    }
 


};
 