namespace ClinicService;

public interface IDoctorSlotAddRepository : IRepository<Slot>
{

}

public sealed class DoctorSlotAddRepository : GenericRepository<Slot>, IDoctorSlotAddRepository
{

    public DoctorSlotAddRepository(ClinicDbContext _dbContext) : base(_dbContext) { }




}