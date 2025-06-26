namespace ClinicService;

public interface IDoctorSlotAddRepository : IRepository<Slot>
{
    Task<bool> HasOverlappingSlots(Guid ClinicId, Guid DoctorId, DateTime startTime, DateTime endTime);
}

public sealed class DoctorSlotAddRepository : GenericRepository<Slot>, IDoctorSlotAddRepository
{

    public DoctorSlotAddRepository(ClinicDbContext _dbContext) : base(_dbContext) { }


    public async Task<bool> HasOverlappingSlots(Guid ClinicId, Guid DoctorId, DateTime startTime, DateTime endTime)
    {
        return await _context.Set<Slot>()
            .AnyAsync(s => s.ClinicId == ClinicId &&
            s.DoctorId == DoctorId &&
            s.StartTime < endTime && s.EndTime > startTime &&
            s.IsActive == true

        );
    }

}