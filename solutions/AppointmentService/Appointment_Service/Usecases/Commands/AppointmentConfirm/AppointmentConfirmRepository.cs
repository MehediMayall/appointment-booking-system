namespace AppointmentService;

public interface IAppointmentConfirmRepository : IRepository<Appointment>
{

}

public sealed class AppointmentConfirmRepository : GenericRepository<Appointment>, IAppointmentConfirmRepository
{

    public AppointmentConfirmRepository(AppointmentDbContext _dbContext) : base(_dbContext) { }




}