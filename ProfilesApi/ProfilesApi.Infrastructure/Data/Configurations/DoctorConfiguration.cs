using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasQueryFilter(a => a.IsActive);
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.GapInMonths)
            .HasColumnType("smallint");

        builder.Property(d => d.Degree)
            .HasMaxLength(30);
        
        builder.HasOne<Specialization>()
            .WithMany()
            .HasForeignKey(d => d.SpecializationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Office>()
            .WithMany()
            .HasForeignKey(d => d.OfficeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Office>()
            .WithOne()
            .HasForeignKey<Doctor>(d => d.OfficeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(d => d.SpecializationId);
        
        builder.HasIndex(d => d.AccountId)
            .IsUnique();
        
        builder.HasIndex(d => d.OfficeId);
    }
}