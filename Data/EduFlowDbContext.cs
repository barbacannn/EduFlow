using EduFlow.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Api.Data;

public class EduFlowDbContext : IdentityDbContext<ApplicationUser>
{
    public EduFlowDbContext(DbContextOptions<EduFlowDbContext> options) : base(options) {}

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
}