namespace PatientService;

public sealed record PatientAddRequestDto()
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


    public Patient New()
    {
        return new Patient()
        {
            Id = Guid.NewGuid(),
            FirstName = FirstName,
            LastName = LastName,
            DateOfBirth = DateOfBirth,
            Gender = Gender,
            BloodType = BloodType,
            Address = Address,
            ContactInfo = ContactInfo,
            EmergencyContacts = EmergencyContacts
        };
    } 
};
 