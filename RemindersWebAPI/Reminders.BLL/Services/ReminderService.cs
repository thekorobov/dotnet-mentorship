using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.Services;

public class ReminderService : IReminderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReminderService(IUnitOfWork unitOfWork, IServiceScopeFactory serviceScopeFactory)
    {
        _unitOfWork = unitOfWork;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task CheckRemindersAndSendEmailsAsync()
    {
        var verifiedUserIds = await _unitOfWork.VerificationCodes.GetQueryable()
            .Where(v => v.VerifiedAt.HasValue)
            .Select(v => v.UserId)
            .ToListAsync();

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var remindersForToday = await _unitOfWork.Reminders.GetQueryable()
            .Where(r => r.Date >= today && r.Date < tomorrow && verifiedUserIds.Contains(r.UserId))
            .ToListAsync();

        var groupedReminders = remindersForToday.GroupBy(r => r.UserId);

        var emailTasks = groupedReminders.Select(async group =>
        {
            using (var innerScope = _serviceScopeFactory.CreateScope())
            {
                var innerUnitOfWork = innerScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var innerEmailService = innerScope.ServiceProvider.GetRequiredService<IEmailService>();

                var user = await innerUnitOfWork.Users.GetAsync(new UserFilter { Id = group.Key });
                if (user == null)
                {
                    throw new EntityNotFoundException(nameof(User));
                }

                var email = user.Email;
                var subject = "Your Reminders for Today";
                var body = BuildEmailBody(group);

                await innerEmailService.SendEmailAsync(email, subject, body);
            }
        }).ToArray();

        await Task.WhenAll(emailTasks);
    }

    private string BuildEmailBody(IGrouping<int, Reminder> group)
    {
        var body = "<strong>Your reminders for today are:</strong><br>";
        var counter = 1;
        foreach (var reminder in group)
        {
            body += $"{counter}) Name - {reminder.Name}, Deadline - {reminder.Date:dd.MM.yyyy HH:mm}<br>";
            counter++;
        }

        return body;
    }
}