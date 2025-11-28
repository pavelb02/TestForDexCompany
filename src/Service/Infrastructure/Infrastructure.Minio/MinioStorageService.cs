using Microsoft.Extensions.Configuration;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;

namespace Infrastructure.Minio;

public class MinioStorageService
{
    private readonly IMinioClient _minio;
    private readonly string _bucketName;

    public MinioStorageService(IConfiguration configuration)
    {
        var section = configuration.GetSection("Minio");

        _bucketName = section["Bucket"] ?? "images";

        _minio = new MinioClient()
            .WithEndpoint(section["Endpoint"] ?? "localhost:9000")
            .WithCredentials(section["AccessKey"], section["SecretKey"])
            .WithSSL(bool.Parse(section["UseSSL"] ?? "false"))
            .Build();
    }

    private async Task EnsureBucketExistsAsync()
    {
        bool found = await _minio.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName)
        );

        if (!found)
            await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
    }

    public async Task<string> UploadAsync(string objectName, Stream data, long size, string contentType)
    {
        await EnsureBucketExistsAsync();

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(data)
            .WithObjectSize(size)
            .WithContentType(contentType));

        return objectName;
    }

    public async Task<Stream> DownloadAsync(string? objectName)
    {
        var ms = new MemoryStream();

        await _minio.GetObjectAsync(new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(ms)));

        ms.Position = 0;
        return ms;
    }

    public async Task DeleteAsync(string objectName)
    {
        await _minio.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName));
    }

    public async Task<IEnumerable<string>> ListAsync()
    {
        var list = new List<string>();
        var observable = _minio.ListObjectsAsync(new ListObjectsArgs().WithBucket(_bucketName));

        var tcs = new TaskCompletionSource<bool>();

        observable.Subscribe(
            item => list.Add(item.Key),
            ex => tcs.TrySetException(ex),
            () => tcs.TrySetResult(true)
        );

        await tcs.Task;
        return list;
    }
}