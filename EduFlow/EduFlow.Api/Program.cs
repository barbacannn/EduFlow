using EduFlow.Api.Data;
using EduFlow.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext
builder.Services.AddDbContext<EduFlowDbContext>(options =>
    options.UseSqlite("Data Source=eduflow.db"));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<EduFlowDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.MapGet("/", () => "EduFlow API is running");

app.Run();