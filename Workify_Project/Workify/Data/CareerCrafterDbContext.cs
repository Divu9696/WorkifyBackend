using System;
using Microsoft.EntityFrameworkCore;
using Workify.Models;

namespace Workify.Data;

public class CareerCrafterDbContext : DbContext
{
    public CareerCrafterDbContext(DbContextOptions<CareerCrafterDbContext> options)
        : base(options)
    {
    }

    // DbSets for entities
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<JobSeeker> JobSeekers { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<JobListing> JobListings { get; set; }
    public DbSet<Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply EntityTypeConfigurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CareerCrafterDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Role>().HasData(
        new Role { Id = 1, RoleName = "Employer", Description = "Can post and manage jobs" },
        new Role { Id = 2, RoleName = "JobSeeker", Description = "Can apply for jobs" },
        new Role { Id = 3, RoleName = "Admin", Description = "Can manage the platform" }
        );


        
    }
        
}

