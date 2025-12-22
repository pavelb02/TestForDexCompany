using Application.Services.DTO;

namespace Application.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Создаёт нового пользователя.
    /// </summary>
    Task<Guid> CreateUserAsync(CreateUserRequest createRequest);

    /// <summary>
    /// Обновляет данные существующего пользователя.
    /// </summary>
    public Task<Guid> UpdateUserAsync(Guid userId, UpdateUserRequest updateRequest);
    
    /// <summary>
    /// Возвращает DTO пользователя.
    /// </summary>
    public Task<UserDto> GetUserAsync(Guid userId);
    
    /// <summary>
    /// Удаляет пользователя.
    /// </summary>
    public Task DeleteUserAsync(Guid userId);
    
    /// <summary>
    /// Добавляет новое объявление пользователю.
    /// </summary>
    public Task AddAdvertisementAsync(Guid userId, CreateAdvertisementRequest createAdvertisementRequest);
    
    /// <summary>
    /// Обновляет данные объявления пользователя.
    /// </summary>
    public Task<Guid> UpdateAdvertisementAsync(Guid userId, Guid advertisementId,
           UpdateAdvertisementRequest updateAdvertisementRequest);
    
    /// <summary>
    /// Удаляет объявление пользователя.
    /// </summary>
    public Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId);
   
    /// <summary>
    /// Устанавливает рейтинг объявлению пользователя.
    /// </summary>
    public Task SetRatingAsync(Guid userId, Guid advertisementId, RatingRequest rating);
    
    /// <summary>
    /// Возвращает имя изображения объявления.
    /// </summary>
    public Task<string> GetImageNameAsync(Guid personId, Guid advertisementId);
}