using Infrastructure.Minio;
using Personnel.Application.Services.Interfaces;

namespace Application.Services.Services;

public class ImageService : IImageService
{
    private readonly MinioStorageService _storage;

    public ImageService(MinioStorageService storage)
    {
        _storage = storage;
    }

    public async Task<string> UploadImageAsync(Stream stream, string fileName, string contentType)
    {
        var imageName = $"{Guid.NewGuid()}-{fileName}";

        var filePath = await _storage.UploadAsync(imageName, stream, stream.Length, contentType);

        return filePath;
    }

    public async Task<(Stream, string)?> DownloadImageAsync(string fileName)
    {
        var stream = await _storage.DownloadAsync(fileName);
        return (stream, fileName);
    }

    public async Task DeleteImageAsync(string imageName)
    {
        await _storage.DeleteAsync(imageName);
    }
}
