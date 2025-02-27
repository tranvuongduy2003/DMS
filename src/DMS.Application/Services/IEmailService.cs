namespace DMS.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);

    Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments);

    Task SendTemplatedEmailAsync<T>(string to, string templateName, T model) where T : class;

    bool ValidateEmailConfiguration();

    Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body);
}

public record EmailAttachment(string FileName, Stream Content, string ContentType);
