namespace DMS.Domain.Settings;

public class EmailOptions
{
    public static string Key = "Email";

    public string Email { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public string DisplayName { get; set; }

    public int Port { get; set; }
}
