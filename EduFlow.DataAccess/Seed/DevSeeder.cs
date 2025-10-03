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
        if (!db.Courses.Any())
        {
            db.Courses.Add(new Course
            {
                Code = "DEV101",
                Name = "Dev DB seeded",
                TeacherId = Guid.NewGuid()
            });
            await db.SaveChangesAsync();
        }
    }
}