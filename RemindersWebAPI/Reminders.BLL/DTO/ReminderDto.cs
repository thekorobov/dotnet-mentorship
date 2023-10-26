using Reminders.DAL.Entities;

namespace Reminders.BLL.DTO;

public record ReminderDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public User User { get; set; }
}