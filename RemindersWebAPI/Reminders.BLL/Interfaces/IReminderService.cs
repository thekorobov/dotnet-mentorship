namespace Reminders.BLL.Interfaces;

public interface IReminderService
{
    Task CheckRemindersAndSendEmailsAsync();
}