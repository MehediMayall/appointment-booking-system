namespace ClinicService;

public interface IDoctorAddRepository : IRepository<Doctor>
{

}

public sealed class DoctorAddRepository : GenericRepository<Doctor>, IDoctorAddRepository
{

    public DoctorAddRepository(ClinicDbContext _dbContext) : base(_dbContext) { }




}