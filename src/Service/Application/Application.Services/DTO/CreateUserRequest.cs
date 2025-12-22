namespace Application.Services.DTO;

public class CreateUserRequest
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string Name { get; init; }
}