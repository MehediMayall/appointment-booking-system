namespace ClinicService;

public sealed record ClinicAddRequestDto()
{
    public string Name { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }


    public Clinic New()
    {
        return new Clinic()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Address = Address,
            PhoneNumber = PhoneNumber
        };
    } 
};
 