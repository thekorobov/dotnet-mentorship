namespace Reminders.BLL.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string emailTo, string subject, string body);
}