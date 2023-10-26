namespace Reminders.BLL.CQS.Reminders.Commands.DeleteReminder;

public record DeleteReminderCommand
{
    public int Id { get; set; }
    public int UserId { get; set; }
}