using System.Text.Json.Serialization;
using Reminders.BLL.Interfaces;

namespace Reminders.BLL.CQS.Users.Commands.UpdateUser;

public record ResetUserPasswordCommand : IValidationRequiredCommand
{
    public int Id { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public int CurrentUserId { get; set; } 
}