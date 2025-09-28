using EduFlow.DataAccess;
using EduFlow.DataAccess.Entities;
using EduFlow.Services.Abstractions;
using EduFlow.Services.Modules.Courses;
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

builder.Services.AddDbContext<EduFlowDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
    
builder.Services
    .AddIdentityCore<ApplicationUser>(o => o.User.RequireUniqueEmail = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EduFlowDbContext>()
    .AddDefaultTokenProviders();
        
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddDataProtection();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await DevSeeder.SeedAsync(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.MapGet("/internal/courses", async (EduFlowDbContext db) =>
        await db.Courses
            .Select(c => new { c.Id, c.Code, c.Name, c.TeacherId })
            .ToListAsync());
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EduFlowDbContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/", () => "EduFlow API (SQLite)").WithTags("Health");

app.Run();