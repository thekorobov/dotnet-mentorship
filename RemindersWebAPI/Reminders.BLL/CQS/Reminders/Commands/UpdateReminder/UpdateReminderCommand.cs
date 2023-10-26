using System.Text.Json.Serialization;
using Reminders.BLL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;

public record UpdateReminderCommand : IValidationRequiredCommand
{
    public int Id { get; set; }
    [JsonIgnore] 
    public int UserId { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
}