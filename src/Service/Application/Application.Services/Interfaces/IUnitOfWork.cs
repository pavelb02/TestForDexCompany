using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Services.Interfaces;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Репозиторий пользователей.
    /// </summary>
    IUserRepository Users { get; }
    
    /// <summary>
    /// Сохраняет все накопленные изменения в базе данных.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Начинает транзакцию с уровнем изоляции Serializable.
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable);
}