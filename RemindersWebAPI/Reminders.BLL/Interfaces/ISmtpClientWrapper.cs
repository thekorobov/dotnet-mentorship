using MailKit.Security;
using MimeKit;

namespace Reminders.BLL.Interfaces;

public interface ISmtpClientWrapper
{
    Task ExecuteEmailActionsAsync(string host, int port, SecureSocketOptions options,
        string username, string password, MimeMessage message);
}