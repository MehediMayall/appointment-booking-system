namespace ClinicService;

public sealed class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Slot> Slots { get; set; }
    public DbSet<DoctorClinic> DoctorClinics { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicDbContext).Assembly); 

        base.OnModelCreating(modelBuilder);
    }
}