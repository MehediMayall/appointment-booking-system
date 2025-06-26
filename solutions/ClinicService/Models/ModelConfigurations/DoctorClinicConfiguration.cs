using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicService;

public sealed class DoctorClinicConfiguration : IEntityTypeConfiguration<DoctorClinic>
{
    public void Configure(EntityTypeBuilder<DoctorClinic> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.JoinedDate).IsRequired();

        // Index
        builder.HasIndex(d => new { d.DoctorId, d.ClinicId }).HasDatabaseName("IX_SlotDoctorClinic").IsUnique();
    }
}