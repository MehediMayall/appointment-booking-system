namespace ClinicService;

public interface IClinicDoctorAddRepository : IRepository<DoctorClinic>
{

}

public sealed class ClinicDoctorAddRepository : GenericRepository<DoctorClinic>, IClinicDoctorAddRepository
{

    public ClinicDoctorAddRepository(ClinicDbContext _dbContext) : base(_dbContext) { }




}