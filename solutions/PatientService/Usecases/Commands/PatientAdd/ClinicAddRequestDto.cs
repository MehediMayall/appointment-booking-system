namespace PatientService;

public sealed record PatientAddRequestDto()
{
    public string Name { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }


    public Patient New()
    {
        return new Patient()
        {
            Id = Guid.NewGuid(),
        };
    } 
};
 