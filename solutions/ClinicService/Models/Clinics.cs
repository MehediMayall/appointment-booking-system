namespace ClinicService;

public class Clinic : EntityBase<Guid>
{

    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public ICollection<Slot> Slots { get; set; }
    public ICollection<Doctor> Doctors { get; set; }

}
