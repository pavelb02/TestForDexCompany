using Application.Services.DTO;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface IUserRepository
{
    Task<Guid> CreateAsync(User user);
    Task<Guid> UpdateAsync(User user);
    Task<User> GetByIdAsync(Guid userId, bool trackChanges);
    Task Delete(User user);
    Task<List<Advertisement>> SearchAsync(AdvertisementSearchRequest request);
}