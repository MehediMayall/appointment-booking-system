namespace ClinicService;

public sealed record ClinicDoctorAddRequestDto()
{
    public Guid ClinicId { get; init; }
    public Guid DoctorId { get; init; }
    public DateTime JoiningDate { get; init; }
    public string Position { get; init; }
   
    public DoctorClinic New()
    {
        return new DoctorClinic()
        {
            Id = Guid.NewGuid(),
            DoctorId = DoctorId,
            ClinicId = ClinicId,
            JoinedDate = JoiningDate.ToUniversalTime(),
            Position = Position             
        };
    } 
};
 