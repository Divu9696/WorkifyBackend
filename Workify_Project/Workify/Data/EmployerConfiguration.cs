using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Models;

namespace Workify.Data;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CompanyDescription)
            .HasMaxLength(500);

        builder.Property(e => e.ContactEmail)
            .IsRequired()
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(e => e.User)
            .WithMany()
             .HasForeignKey(e => e.UserId) // No need for `HasForeignKey` when directly using navigation property
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete employer

        builder.HasMany(e => e.JobListings)
            .WithOne(jl => jl.Employer)
            .HasForeignKey(jl => jl.EmployerId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete job listings
    }
}
