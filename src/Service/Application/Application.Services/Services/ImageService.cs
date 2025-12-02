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

    public async Task<string> UploadImageAsync(Stream stream, string fileName, string contentType)
    {
        var imageName = $"{Guid.NewGuid()}-{fileName}";

        await _storage.UploadAsync($"original/{imageName}", stream, stream.Length, contentType);
        
        stream.Position = 0; 
        using var img = await Image.LoadAsync(stream);

        int width = 300;
        int height = (int)(img.Height / (img.Width / (double)width));

        img.Mutate(x => x.Resize(width, height));

        var smallStream = new MemoryStream();
        await img.SaveAsJpegAsync(smallStream, new JpegEncoder());
        smallStream.Position = 0;

        await _storage.UploadAsync($"small/{imageName}", smallStream, smallStream.Length, "image/jpeg");

        return imageName;
    }
    
    public async Task<(Stream Stream, string FileName)> GetResizedImageAsync(string size, string fileName)
    {
        string key;
        if (size == "small") key = $"small/{fileName}";
        else if (size == "original") key = $"original/{fileName}";
        else throw new ArgumentException("Размер должен быть 'small' или 'original'.");

        var stream = await _storage.DownloadAsync(key);
        return (stream, fileName);
    }

    public async Task DeleteImageAsync(string imageName)
    {
        await _storage.DeleteAsync($"original/{imageName}");
        await _storage.DeleteAsync($"small/{imageName}");
    }
}