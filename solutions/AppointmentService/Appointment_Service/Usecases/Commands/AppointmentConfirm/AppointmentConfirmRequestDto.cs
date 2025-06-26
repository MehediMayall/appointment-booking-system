using Appointment_Contracts;

namespace AppointmentService;

public sealed record AppointmentConfirmRequestDto()
{
    public Guid AppointmentId { get; set; }

};
 