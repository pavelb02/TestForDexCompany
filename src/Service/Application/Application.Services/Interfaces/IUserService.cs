using Application.Services.DTO;

namespace Application.Services.Interfaces;

public interface IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserRequest createRequest);
    public Task<Guid> UpdateUserAsync(UpdateUserRequest updateRequest);
    public Task<UserDto> GetUserAsync(Guid userId);
    public Task DeleteUserAsync(Guid userId);

    public Task AddAdvertisementAsync(Guid userId, CreateAdvertisementDto createAdvertisementDto);
    public Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId);
    public Task<Guid> UpdateAdvertisementAsync(Guid userId, UpdateAdvertisementRequest updateAdvertisementRequest);
}