using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BMS.Infrastructure.Data;

public class BMSDBContextFactory : IDesignTimeDbContextFactory<BMSDBContext>
{
    public BMSDBContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("BMS_ConnectionStrings__Default")
                               ?? "Host=localhost;Port=5432;Database=bms;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<BMSDBContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new BMSDBContext(optionsBuilder.Options);
    }
}