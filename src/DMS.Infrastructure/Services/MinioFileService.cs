using DMS.Application.Services;
using Minio;
using Minio.DataModel.Args;

namespace DMS.Infrastructure.Services;

public class MinioFileService : IFileService
{
    private readonly IMinioClient _minioClient;

    public MinioFileService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<string> UploadFileAsync(string containerName, Stream stream, string fileName, string contentType)
    {
        string fileId = Guid.NewGuid().ToString();
        PutObjectArgs putObjectArgs = new PutObjectArgs()
            .WithBucket(containerName)
            .WithObject(fileId)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType)
            .WithHeaders(new Dictionary<string, string>
            {
                ["x-amz-meta-filename"] = fileName,
                ["x-amz-meta-contenttype"] = contentType
            });

        await _minioClient.PutObjectAsync(putObjectArgs);
        return fileId;
    }

    public async Task DeleteFileAsync(string containerName, string fileId)
    {
        RemoveObjectArgs removeArgs = new RemoveObjectArgs()
            .WithBucket(containerName)
            .WithObject(fileId);

        await _minioClient.RemoveObjectAsync(removeArgs);
    }

    public async Task<FileMetadata> GetFileInfoAsync(string containerName, string fileId)
    {
        StatObjectArgs statObjectArgs = new StatObjectArgs()
            .WithBucket(containerName)
            .WithObject(fileId);

        Minio.DataModel.ObjectStat stats = await _minioClient.StatObjectAsync(statObjectArgs);

        return new FileMetadata
        {
            FileId = fileId,
            FileName = stats.MetaData.TryGetValue("x-amz-meta-filename", out string? value) ? value : fileId,
            ContentType = stats.ContentType,
            FileSize = stats.Size,
            CreatedAt = stats.LastModified
        };
    }
}
