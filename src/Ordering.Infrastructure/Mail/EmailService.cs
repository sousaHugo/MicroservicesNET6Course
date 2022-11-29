using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail;

public class EmailService : IEmailService
{
    public EmailSettings _emailSettings { get; }
    private readonly ILogger<EmailService> _logger;
    public EmailService(IOptions<EmailSettings> EmailSettings, ILogger<EmailService> Logger)
    {
        _emailSettings = EmailSettings.Value ?? throw new ArgumentNullException(nameof(EmailSettings));
        _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
    }
    public async Task<bool> SendEmail(Email Email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var subject = Email.Subject;
        var to = new EmailAddress(Email.To);
        var emailBody = Email.Body;

        var from = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName
        };

        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        var response = await client.SendEmailAsync(sendGridMessage);

        _logger.LogInformation("Email sent.");

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            return true;

        _logger.LogError("Email sending failed.");
        return false;
    }
}
