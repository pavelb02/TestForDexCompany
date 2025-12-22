namespace Application.Services.DTO;

public class UserDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Коллекция объявлений пользователя.
    /// </summary>
    public List<AdvertisementDto>? Advertisements { get; init; }
}