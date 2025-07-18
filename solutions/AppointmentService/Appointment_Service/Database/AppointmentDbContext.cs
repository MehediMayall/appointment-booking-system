namespace AppointmentService;

public sealed class AppointmentDbContext : DbContext
{
    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options) { }
    public DbSet<Appointment> Appointments { get; set; }
 
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentDbContext).Assembly); 

        base.OnModelCreating(modelBuilder);
    }
}