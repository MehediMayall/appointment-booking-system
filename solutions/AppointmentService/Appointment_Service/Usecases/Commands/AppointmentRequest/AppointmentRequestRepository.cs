namespace AppointmentService;

public interface IAppointmentRequestRepository : IRepository<Appointment>
{

}

public sealed class AppointmentRequestRepository : GenericRepository<Appointment>, IAppointmentRequestRepository
{

    public AppointmentRequestRepository(AppointmentDbContext _dbContext) : base(_dbContext) { }




}