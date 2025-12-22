using Application.Services.Interfaces;
using Infrastructure.Minio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Application.Services.Services;

public class ImageService : IImageService
{
    private readonly MinioStorageService _storage;
    
    public ImageService(MinioStorageService storage)
    {
        _storage = storage;
    }

    public async Task<string> UploadImageAsync(Stream stream, string fileName, string contentType, int width)
    {
        var imageName = $"{Guid.NewGuid()}-{fileName}";

        await _storage.UploadAsync($"original/{imageName}", stream, stream.Length, contentType);
        
        stream.Position = 0; 
        using var img = await Image.LoadAsync(stream);

        int height = (int)(img.Height / (img.Width / (double)width));

        img.Mutate(x => x.Resize(width, height));

        var resizedStream = new MemoryStream();
        await img.SaveAsJpegAsync(resizedStream, new JpegEncoder());
        resizedStream.Position = 0;

        await _storage.UploadAsync($"resized/{imageName}", resizedStream, resizedStream.Length, "image/jpeg");

        return imageName;
    }
    
    public async Task GetResizedImageAsync(string size, string fileName, Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        
        string key;
        if (size == "resized") key = $"resized/{fileName}";
        else if (size == "original") key = $"original/{fileName}";
        else throw new ArgumentException("Размер должен быть 'resized' или 'original'.");
        
        await _storage.DownloadAsync(key, stream, cancellationToken);
    }

    public async Task DeleteImageAsync(string imageName)
    {
        await _storage.DeleteAsync($"original/{imageName}");
        await _storage.DeleteAsync($"resized/{imageName}");
    }
}