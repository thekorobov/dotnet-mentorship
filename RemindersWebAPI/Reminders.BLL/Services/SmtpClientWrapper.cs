using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Reminders.BLL.Interfaces;

namespace Reminders.BLL.Services;

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