using Application.Services.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TestForDexCompanyDbContext _dbContext;
    public IUserRepository Users { get; }

    public UnitOfWork(TestForDexCompanyDbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        Users = userRepository;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}