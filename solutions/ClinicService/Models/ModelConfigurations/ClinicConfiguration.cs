using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicService;

public sealed class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
{
    public void Configure(EntityTypeBuilder<Clinic> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Address).IsRequired().HasMaxLength(250);

        // Relations
        builder.HasMany(c => c.Slots).WithOne(c => c.clinic).HasForeignKey(c => c.ClinicId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Doctors)
            .WithMany(c => c.Clinics)
            .UsingEntity<DoctorClinic>(
                dc => dc.HasOne(d => d.Doctor).WithMany().HasForeignKey(d => d.DoctorId).OnDelete(DeleteBehavior.Cascade),
                dc => dc.HasOne(c => c.Clinic).WithMany().HasForeignKey(c => c.ClinicId).OnDelete(DeleteBehavior.Cascade)
            );


        // Index
        builder.HasIndex(d=> new { d.Name, d.Address} ).HasDatabaseName("IX_NameAddress").IsUnique();
        builder.HasIndex(d=> new { d.Name, d.IsActive } ).HasDatabaseName("IX_NameIsActive").IsUnique();

        builder.HasIndex(d => d.Name).HasDatabaseName("IX_Name");
        builder.HasIndex(d => d.PhoneNumber).HasDatabaseName("IX_PhoneNumber").IsUnique();
    }
}
