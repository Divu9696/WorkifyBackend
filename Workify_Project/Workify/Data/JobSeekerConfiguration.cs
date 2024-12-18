using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Models;

namespace Workify.Data;

public class JobSeekerConfiguration : IEntityTypeConfiguration<JobSeeker>
{
    public void Configure(EntityTypeBuilder<JobSeeker> builder)
    {
        // Primary Key
        builder.HasKey(js => js.Id);

        // Properties
        builder.Property(js => js.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(js => js.ContactNumber)
            .HasMaxLength(15);

        builder.Property(js => js.Education)
            .HasMaxLength(200);

        builder.Property(js => js.Skills)
            .HasMaxLength(500);

        builder.Property(js => js.ResumePath)
            .IsRequired()
            .HasMaxLength(200);

        // Relationships
        builder.HasOne(js => js.User)
            .WithMany()
            .HasForeignKey(js => js.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete job seeker

        builder.HasMany(js => js.Applications)
            .WithOne(a => a.JobSeeker)
            .HasForeignKey(a => a.JobSeekerId)
            .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete for applications
    }
}
