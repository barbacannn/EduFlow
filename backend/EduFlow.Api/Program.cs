using EduFlow.DataAccess;
using EduFlow.DataAccess.Entities;
using EduFlow.Services.Abstractions;
using EduFlow.Services.Modules.Courses;
using EduFlow.Services.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EduFlow API", Version = "v1" });
});

var solutionRoot = Directory.GetParent(builder.Environment.ContentRootPath)!.FullName;
var dbPath = Path.Combine(solutionRoot, ".data", "eduflow.dev.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

builder.Services.AddDbContext<EduFlowDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services
    .AddIdentityCore<ApplicationUser>(o => o.User.RequireUniqueEmail = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EduFlowDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDataProtection();
builder.Services.AddScoped<IDataSeeder, DevSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    var db  = scope.ServiceProvider.GetRequiredService<EduFlowDbContext>();

    db.Database.Migrate();

    if (env.IsDevelopment())
    {
        await DevIdentitySeeder.SeedAsync(app.Services);

        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync(db);
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // dev-only helper endpoint
    app.MapGet("/internal/courses", async (EduFlowDbContext db) =>
        await db.Courses
            .Select(c => new { c.Id, c.Code, c.Name, c.TeacherId })
            .ToListAsync());
}

app.MapControllers();
app.MapGet("/", () => "EduFlow API (SQLite)").WithTags("Health");

app.Run();
