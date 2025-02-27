namespace DMS.Domain.Settings;

public class MinioStorage
{
    public static string Key = "Minio";

    public string Endpoint { get; set; } = string.Empty;

    public string AccessKey { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public bool Secure { get; set; }
}

