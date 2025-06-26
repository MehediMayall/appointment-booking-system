namespace ClinicService;

public class Doctor : EntityBase<Guid>
{

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Specialization { get; set; }
    public string Email { get; set; }


    public ICollection<Slot> Slots { get; set; }
    public ICollection<Clinic> Clinics { get; set; }

}
