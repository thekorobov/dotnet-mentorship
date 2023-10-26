using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;

namespace TicketFlow.Infrastructure.Shared.Services.Email.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ISmtpClientWrapper _smtpClientWrapper;
    
    public EmailService(IConfiguration configuration, ISmtpClientWrapper smtpClientWrapper)
    {
        _configuration = configuration;
        _smtpClientWrapper = smtpClientWrapper;
    }

    public async Task SendEmailAsync(string emailTo, string subject, string body)
    {
        if (string.IsNullOrEmpty(emailTo) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
        {
            throw new ArgumentException("Email, subject and body must be provided.");
        }

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Mailkit:EmailUsername"]));
        email.To.Add(MailboxAddress.Parse(emailTo));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        await _smtpClientWrapper.ExecuteEmailActionsAsync(
            _configuration["Mailkit:EmailHost"]!,
            587,
            SecureSocketOptions.StartTls,
            _configuration["Mailkit:EmailUsername"]!,
            _configuration["Mailkit:EmailPassword"]!,
            email
        );
    }
}