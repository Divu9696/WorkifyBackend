using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Models;

namespace Workify.Data;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        // Primary Key
        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.AppliedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(a => a.JobSeeker)
            .WithMany(js => js.Applications)
            .HasForeignKey(a => a.JobSeekerId)
            .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete for job seekers

        builder.HasOne(a => a.JobListing)
            .WithMany(jl => jl.Applications)
            .HasForeignKey(a => a.JobListingId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for job listings
    }
}
