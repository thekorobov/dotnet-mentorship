using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;

namespace TicketFlow.Infrastructure.Shared.Services.Email.Implementations;

public class SmtpClientWrapper : ISmtpClientWrapper
{
    public async Task ExecuteEmailActionsAsync(string host, int port, SecureSocketOptions options,
        string username, string password, MimeMessage message)
    {
        using (var smtpClient = new SmtpClient())
        {
            await smtpClient.ConnectAsync(host, port, options);
            await smtpClient.AuthenticateAsync(username, password);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }
    }
}