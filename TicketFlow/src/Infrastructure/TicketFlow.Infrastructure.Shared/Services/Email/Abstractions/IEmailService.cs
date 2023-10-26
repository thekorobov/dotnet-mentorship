namespace TicketFlow.Infrastructure.Shared.Services.Email.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string emailTo, string subject, string body);
}