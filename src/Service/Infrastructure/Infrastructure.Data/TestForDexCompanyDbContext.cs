using System.Reflection;
using Domain.Entities;
using Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TestForDexCompanyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }

    public TestForDexCompanyDbContext(DbContextOptions<TestForDexCompanyDbContext> options)
        : base(options)
    {
    }

    public TestForDexCompanyDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null) throw new ArgumentException(nameof(modelBuilder));

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}