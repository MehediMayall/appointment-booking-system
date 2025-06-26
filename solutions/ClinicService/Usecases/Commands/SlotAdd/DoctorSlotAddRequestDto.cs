namespace ClinicService;

public sealed record DoctorSlotAddRequestDto()
{
    public Guid ClinicId { get; init; }
    public Guid DoctorId { get; init; }
    public DateOnly Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }

   
    public Slot New()
    {
        DateTime startTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hours, StartTime.Minutes, 0);
        DateTime endTime = new DateTime(Date.Year, Date.Month, Date.Day, EndTime.Hours, EndTime.Minutes,0);

        return new Slot()
        {
            Id = Guid.NewGuid(),
            DoctorId = DoctorId,
            ClinicId = ClinicId,
            StartTime = startTime.ToUniversalTime(),
            EndTime = endTime.ToUniversalTime(),
            IsBooked = false
        };
    } 
};
 