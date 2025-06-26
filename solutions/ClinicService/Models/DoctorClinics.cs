namespace ClinicService;

public class DoctorClinic : EntityBase<Guid>
{
    // Clinic
    public Guid ClinicId { get; set; }
    public Clinic Clinic { get; set; }

    // Doctor

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    // Join Date
    public DateTime JoinedDate { get; set; }
    public string Position { get; set; }
}