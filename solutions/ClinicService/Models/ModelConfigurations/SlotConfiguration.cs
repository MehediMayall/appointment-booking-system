using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicService;

public sealed class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.StartTime).IsRequired();
        builder.Property(d => d.EndTime).IsRequired();



        builder.HasIndex(d => d.StartTime).HasDatabaseName("IX_StartTime");
        builder.HasIndex(d => d.EndTime).HasDatabaseName("IX_EndTime");

        builder.HasIndex(d => new { d.DoctorId, d.ClinicId }).HasDatabaseName("IX_DoctorClinic").IsUnique();
        builder.HasIndex(d => new { d.DoctorId, d.ClinicId, d.IsActive }).HasDatabaseName("IX_DoctorClinicIsActive").IsUnique();
        builder.HasIndex(d => new { d.DoctorId, d.ClinicId, d.IsActive, d.StartTime }).HasDatabaseName("IX_DoctorClinicIsActiveStartTime").IsUnique();
    }
}
