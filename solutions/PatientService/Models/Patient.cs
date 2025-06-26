namespace PatientService;

public sealed class Patient : EntityBase<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } 
    public string BloodType { get; set; }
    public Address Address { get; set; }
    public ContactInfo ContactInfo { get; set; }
    public List<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
    public InsuranceInformation InsuranceInfo { get; set; }
    public List<string> Allergies { get; set; } = new List<string>();
    public List<string> Medications { get; set; } = new List<string>();
    public List<MedicalRecord> MedicalHistory { get; set; } = new List<MedicalRecord>();

}