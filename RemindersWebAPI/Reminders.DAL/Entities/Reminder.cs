namespace Reminders.DAL.Entities;

public class Reminder
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public virtual User User { set; get; }
}