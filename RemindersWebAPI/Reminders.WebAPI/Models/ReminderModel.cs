namespace Reminders.WebAPI.Models;

public class ReminderModel
{
    public int Id { get; set; } 
    public string? Name { get; set; }
    public DateTime Date { get; set; }
}