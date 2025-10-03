using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EduFlow.DataAccess;

public class EduFlowDbContextFactory : IDesignTimeDbContextFactory<EduFlowDbContext>
{
    public EduFlowDbContext CreateDbContext(string[] args)
    {
        var dataDir = Path.Combine(Directory.GetCurrentDirectory(), ".data");
        Directory.CreateDirectory(dataDir);
        var dbPath = Path.Combine(dataDir, "eduflow.dev.db");

        var options = new DbContextOptionsBuilder<EduFlowDbContext>()
            .UseSqlite($"Data Source={dbPath}",
                b => b.MigrationsAssembly("EduFlow.DataAccess"))
            .Options;

        return new EduFlowDbContext(options);
    }
}