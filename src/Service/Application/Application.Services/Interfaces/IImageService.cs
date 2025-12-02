namespace Application.Services.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream stream, string fileName, string contentType);
    Task DeleteImageAsync(string imageName);
    Task<(Stream Stream, string FileName)> GetResizedImageAsync(string fileName, string size);
}