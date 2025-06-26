using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicService;

public sealed class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.LastName).IsRequired().HasMaxLength(100);

        // Relations
        builder.HasMany(d => d.Slots).WithOne(d => d.doctor).HasForeignKey(d => d.DoctorId).OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(d=> d.LastName).HasDatabaseName("IX_LastName").IsUnique();
        builder.HasIndex(d=> d.Email).HasDatabaseName("IX_Email").IsUnique();
    }
}
