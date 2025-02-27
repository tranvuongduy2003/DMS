namespace DMS.Domain.Settings;

public class JwtOptions
{
    public static string Key = "Jwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;
}

