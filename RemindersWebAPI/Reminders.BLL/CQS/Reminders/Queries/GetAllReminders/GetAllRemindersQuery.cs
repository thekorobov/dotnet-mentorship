namespace Reminders.BLL.CQS.Reminders.Queries.GetAllReminders;

public record GetAllRemindersQuery
{
    public int UserId { get; set; }
}