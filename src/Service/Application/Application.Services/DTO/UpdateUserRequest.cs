namespace Application.Services.DTO;

public class UpdateUserRequest
{ 
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string Name { get; init; }
}