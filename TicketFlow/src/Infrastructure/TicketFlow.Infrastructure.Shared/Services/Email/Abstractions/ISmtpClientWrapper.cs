using MailKit.Security;
using MimeKit;

namespace TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;

public interface ISmtpClientWrapper
{
    Task ExecuteEmailActionsAsync(string host, int port, SecureSocketOptions options,
        string username, string password, MimeMessage message);
}