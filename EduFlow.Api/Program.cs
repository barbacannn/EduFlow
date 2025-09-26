using EduFlow.Api.Data;
using EduFlow.Api.Models;
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

builder.Services.AddDbContextPool<EduFlowDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("EduFlow")));
    
    builder.Services
        .AddIdentityCore<ApplicationUser>(o => o.User.RequireUniqueEmail = true)
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<EduFlowDbContext>()
        .AddDefaultTokenProviders();
        
        builder.Services.AddScoped<ICourseService, CourseService>();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.MapControllers();
        app.MapGet("/", () => "EduFlow API (SQLite)").WithTags("Health");
        
        app.Run();