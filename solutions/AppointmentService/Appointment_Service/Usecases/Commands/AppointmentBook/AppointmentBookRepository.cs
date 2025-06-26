namespace AppointmentService;

public interface IAppointmentBookRepository : IRepository<Appointment>
{

}

public sealed class AppointmentBookRepository : GenericRepository<Appointment>, IAppointmentBookRepository
{

    public AppointmentBookRepository(AppointmentDbContext _dbContext) : base(_dbContext) { }




}