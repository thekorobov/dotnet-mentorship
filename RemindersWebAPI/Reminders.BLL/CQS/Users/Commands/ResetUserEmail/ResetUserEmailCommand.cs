using System.Text.Json.Serialization;
using Reminders.BLL.Interfaces;

namespace Reminders.BLL.CQS.Users.Commands.ResetUserEmail;

public class ResetUserEmailCommand : IValidationRequiredCommand
{
    public int Id { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public int CurrentUserId { get; set; } 
}