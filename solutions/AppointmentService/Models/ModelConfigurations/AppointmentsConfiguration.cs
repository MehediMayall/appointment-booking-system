using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentService;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.PatientId).IsRequired();
        builder.Property(d => d.SlotId).IsRequired();
        builder.Property(d => d.DoctorId).IsRequired();
        builder.Property(d => d.Status).IsRequired();
     
    }
}
