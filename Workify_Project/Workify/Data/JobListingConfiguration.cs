using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Models;

namespace Workify.Data;

public class JobListingConfiguration : IEntityTypeConfiguration<JobListing>
{
    public void Configure(EntityTypeBuilder<JobListing> builder)
    {
        // Primary Key
        builder.HasKey(jl => jl.Id);

        // Properties
        builder.Property(jl => jl.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(jl => jl.Description)
            .HasMaxLength(1000);

        builder.Property(jl => jl.Location)
            .HasMaxLength(100);

        builder.Property(jl => jl.JobType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(jl => jl.Qualifications)
            .HasMaxLength(500);

        builder.Property(jl => jl.Salary)
            .HasPrecision(18, 2);

        builder.Property(jl => jl.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(jl => jl.Employer)
            .WithMany(e => e.JobListings)
            .HasForeignKey(jl => jl.EmployerId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete job listings

        builder.HasMany(jl => jl.Applications)
            .WithOne(a => a.JobListing)
            .HasForeignKey(a => a.JobListingId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete applications
    }
}
