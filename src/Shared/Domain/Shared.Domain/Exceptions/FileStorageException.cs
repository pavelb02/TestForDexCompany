namespace Shared.Domain.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при ошибках работы с файловым хранилищем.
/// </summary>
public class FileStorageException : Exception
{
    public FileStorageException()
        : base("Не удалось сохранить файл")
    {
    }

    public FileStorageException(string message)
        : base(message)
    {
    }

    public FileStorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}