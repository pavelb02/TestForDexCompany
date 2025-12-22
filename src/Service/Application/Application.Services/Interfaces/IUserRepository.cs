using Application.Services.DTO;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Добавляет нового пользователя в хранилище.
    /// </summary>
    Task<Guid> CreateAsync(User user);
    
    /// <summary>
    /// Возвращает пользователя по идентификатору.
    /// </summary>
    Task<User> GetByIdAsync(Guid userId, bool trackChanges);
    
    /// <summary>
    /// Удаляет пользователя из хранилища.
    /// </summary>
    Task Delete(User user);
}