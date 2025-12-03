using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class TestForDexCompanyDbContextFactory 
    : IDesignTimeDbContextFactory<TestForDexCompanyDbContext>
{
    public TestForDexCompanyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestForDexCompanyDbContext>();

        // Локальная база для разработки
        var connectionString = "Host=localhost;Port=5432;Database=TestForDexCompany_DB;Username=postgres;Password=postgres;SSL Mode=Disable";

        optionsBuilder.UseNpgsql<TestForDexCompanyDbContext>(connectionString,
            x => x.MigrationsAssembly(typeof(TestForDexCompanyDbContext).Assembly.FullName));

        return new TestForDexCompanyDbContext(optionsBuilder.Options);
    }
}