namespace Reminders.BLL.CQS.Reminders.Queries.GetReminderById;

public record GetReminderByIdQuery
{
    public int Id { get; set; }
    public int UserId { get; set; }
}