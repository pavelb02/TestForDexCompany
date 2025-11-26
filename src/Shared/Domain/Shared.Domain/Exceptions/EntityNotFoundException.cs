namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, выбрасываемое, когда сущность не найдена в хранилище.
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
        : base("Сущность не найдена.")
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}