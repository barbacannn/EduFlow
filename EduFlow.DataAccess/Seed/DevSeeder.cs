using Microsoft.EntityFrameworkCore;
using EduFlow.DataAccess.Entities;

namespace EduFlow.DataAccess;

public interface IDataSeeder
{
    Task SeedAsync(EduFlowDbContext db);
}

public class DevSeeder : IDataSeeder
{
    public async Task SeedAsync(EduFlowDbContext db)
    {
        var teacherId = await db.Users
            .Where(u => u.Email == "teacher@example.com")
            .Select(u => u.Id)
            .FirstOrDefaultAsync();

        if (teacherId == Guid.Empty) return;

        if (!await db.Courses.AnyAsync(c => c.Code == "DEV101"))
        {
            db.Courses.Add(new Course
            {
                Id = Guid.NewGuid(),
                Code = "DEV101",
                Name = "Dev DB seeded",
                TeacherId = teacherId,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                IsDeleted = false
            });

            await db.SaveChangesAsync();
        }
    }
}