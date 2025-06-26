namespace ClinicService;

public interface IClinicAddRepository : IRepository<Clinic>
{

}

public sealed class ClinicAddRepository : GenericRepository<Clinic>, IClinicAddRepository
{

    public ClinicAddRepository(ClinicDbContext _dbContext) : base(_dbContext) { }




}