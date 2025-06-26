namespace ClinicService;

public sealed record DoctorAddRequestDto()
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Specialization { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }


    public Doctor New()
    {
        return new Doctor()
        {
            Id = Guid.NewGuid(),
            FirstName = FirstName,
            LastName = LastName,
            Specialization = Specialization,
            Email = Email,
            PhoneNumber = PhoneNumber
        };
    } 
};
 