using System.Reflection;
using EduFlow.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DataAccess;

public class EduFlowDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public EduFlowDbContext(DbContextOptions<EduFlowDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = Guid.Parse("..."),
            UserName = "teacher@example.com",
            Email = "teacher@example.com"
        });

        modelBuilder.Entity<Course>().HasData(new Course
        {
            Id = Guid.Parse("..."),
            Code = "DEV101",
            Name = "Dev DB seeded",
            TeacherId = Guid.Parse("...")
        });*/
    }


    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in ChangeTracker.Entries<Course>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAtUtc = now;
            if (entry.State == EntityState.Modified) entry.Entity.ModifiedAtUtc = now;
        }
        return base.SaveChangesAsync(ct);
    }
}