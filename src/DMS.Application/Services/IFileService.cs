namespace DMS.Application.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(string containerName, Stream stream, string fileName, string contentType);

    Task DeleteFileAsync(string containerName, string fileId);

    Task<FileMetadata> GetFileInfoAsync(string containerName, string fileId);
}

public record FileMetadata
{
    public required string FileId { get; init; }
    public required string FileName { get; init; }
    public required long FileSize { get; init; }
    public required string ContentType { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}
