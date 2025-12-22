using System.Data;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable)
    {
        return await _dbContext.Database.BeginTransactionAsync(isolationLevel);
    }
}