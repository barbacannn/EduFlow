using EduFlow.DataAccess;
using EduFlow.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DevIdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EduFlowDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        string[] roles = { "Teacher", "Student", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var email = "teacher@example.com";
        var teacher = await userManager.FindByEmailAsync(email);
        if (teacher == null)
        {
            teacher = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var create = await userManager.CreateAsync(teacher, "Dev!Pass123");
            if (!create.Succeeded)
            {
                throw new InvalidOperationException("Failed to create dev user: " +
                                                    string.Join(", ", create.Errors.Select(e => $"{e.Code}:{e.Description}")));
            }
        }

        if (!await userManager.IsInRoleAsync(teacher, "Teacher"))
        {
            await userManager.AddToRoleAsync(teacher, "Teacher");
        }

        if (!await db.Courses.AnyAsync(c => c.Code == "DEV101"))
        {
            db.Courses.Add(new Course
            {
                Id = Guid.NewGuid(),
                Code = "DEV101",
                Name = "New DB seeded",
                TeacherId = teacher.Id
            });
            await db.SaveChangesAsync();
        }
    }
}
