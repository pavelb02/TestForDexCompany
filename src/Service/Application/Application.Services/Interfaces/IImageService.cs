namespace Application.Services.Interfaces;

/// <summary>
/// Предоставляет методы для загрузки, хранения и получения изображений,
/// включая и изменение их размера.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Загружает изображение, при необходимости изменяя его ширину,
    /// и сохраняет его в хранилище.
    /// </summary>
    Task<string> UploadImageAsync(Stream stream, string fileName, string contentType, int width);
    
    /// <summary>
    /// Удаляет изображение из хранилища по его имени.
    /// </summary>
    Task DeleteImageAsync(string imageName);
    
    /// <summary>
    /// Возвращает сжатую версию изображения.
    /// </summary>
    Task GetResizedImageAsync(string size, string fileName, Stream stream, CancellationToken cancellationToken);
}