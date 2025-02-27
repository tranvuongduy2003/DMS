using System.Net.Mail;
using DMS.Application.Services;

namespace DMS.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromAddress;

    public EmailService(SmtpClient smtpClient, string fromAddress)
    {
        _smtpClient = smtpClient;
        _fromAddress = fromAddress;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromAddress),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);

        await _smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments)
    {
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromAddress),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);

        foreach (EmailAttachment attachment in attachments)
        {
            mailMessage.Attachments.Add(new Attachment(attachment.Content, attachment.FileName));
        }

        await _smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendTemplatedEmailAsync<T>(string to, string templateName, T model) where T : class
    {
        // Implementation would typically use a template engine like Razor
        // For now, returning a simple implementation
        string body = $"Template: {templateName}, Model: {model}";
        await SendEmailAsync(to, $"Templated Email: {templateName}", body);
    }

    public bool ValidateEmailConfiguration()
    {
        try
        {
            return _smtpClient != null && !string.IsNullOrEmpty(_fromAddress);
        }
        catch
        {
            return false;
        }
    }

    public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body)
    {
        foreach (string recipient in recipients)
        {
            await SendEmailAsync(recipient, subject, body);
        }
    }
}
