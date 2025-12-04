using Application.Services.DTO;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserRequest createRequest);
    public Task<Guid> UpdateUserAsync(UpdateUserRequest updateRequest);
    public Task<UserDto> GetUserAsync(Guid userId, bool trackChanges);
    public Task DeleteUserAsync(Guid userId);
    public Task<List<AdvertisementDto>> SearchAsync(AdvertisementSearchRequest searchRequest);

    public Task SetRatingAsync(RatingRequest rating);

    public Task AddAdvertisementAsync(Guid userId, CreateAdvertisementRequest createAdvertisementRequest);
    public Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId);
    public Task<Guid> UpdateAdvertisementAsync(Guid userId, UpdateAdvertisementRequest updateAdvertisementRequest);
    public Task<string> GetImageNameAsync(Guid personId, Guid advertisementId);
}