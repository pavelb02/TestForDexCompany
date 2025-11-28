namespace Personnel.Application.Services.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream stream, string fileName, string contentType);
    Task<(Stream, string)?> DownloadImageAsync(string? fileName);
    Task DeleteImageAsync(string imageName);
}