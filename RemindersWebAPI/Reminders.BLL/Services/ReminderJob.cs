using Microsoft.Extensions.DependencyInjection;
using Reminders.BLL.Interfaces;

namespace Reminders.BLL.Services;

public class ReminderJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReminderJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ExecuteAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
            await reminderService.CheckRemindersAndSendEmailsAsync();
        }
    }
}
