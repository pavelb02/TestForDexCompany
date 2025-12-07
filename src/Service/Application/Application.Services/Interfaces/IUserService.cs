using Application.Services.DTO;

namespace Application.Services.Interfaces;

public interface IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserRequest createRequest);
    public Task<Guid> UpdateUserAsync(Guid userId, UpdateUserRequest updateRequest);
    public Task<UserDto> GetUserAsync(Guid userId);
    public Task DeleteUserAsync(Guid userId);
    public Task<List<AdvertisementDto>> SearchAsync(AdvertisementSearchRequest searchRequest);
    public Task AddAdvertisementAsync(Guid userId, CreateAdvertisementRequest createAdvertisementRequest);
    public Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId);
    public Task<Guid> UpdateAdvertisementAsync(Guid userId, Guid advertisementId,
        UpdateAdvertisementRequest updateAdvertisementRequest);
    public Task SetRatingAsync(Guid userId, Guid advertisementId, RatingRequest rating);
    public Task<string> GetImageNameAsync(Guid personId, Guid advertisementId);
}