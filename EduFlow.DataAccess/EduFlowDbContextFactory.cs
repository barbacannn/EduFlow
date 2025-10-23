using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EduFlow.DataAccess;

public class EduFlowDbContextFactory : IDesignTimeDbContextFactory<EduFlowDbContext>
{
    public EduFlowDbContext CreateDbContext(string[] args)
    {
        var apiDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "EduFlow.Api");
        var config = new ConfigurationBuilder()
            .SetBasePath(apiDir)
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var cs = config.GetConnectionString("EduFlowDb")
                 ?? config.GetConnectionString("Default");

        var options = new DbContextOptionsBuilder<EduFlowDbContext>()
            .UseSqlite(cs, b => b.MigrationsAssembly("EduFlow.DataAccess"))
            .Options;

        return new EduFlowDbContext(options);

    }
}