using System.Text.Json.Serialization;
using Reminders.BLL.Interfaces;

namespace Reminders.BLL.CQS.Reminders.Commands.CreateReminder;

public record CreateReminderCommand : IValidationRequiredCommand
{
    [JsonIgnore] 
    public int UserId { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
}