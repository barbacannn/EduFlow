using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EduFlow.DataAccess;

public class EduFlowDbContextFactory : IDesignTimeDbContextFactory<EduFlowDbContext>
{
    public EduFlowDbContext CreateDbContext(string[] args)
    {
        var dbPath = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,
            ".data",
            "eduflow.dev.db"
        );

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        var options = new DbContextOptionsBuilder<EduFlowDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        return new EduFlowDbContext(options);
    }
}