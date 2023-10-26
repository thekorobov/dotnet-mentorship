using System.Text.Json.Serialization;

namespace Reminders.BLL.CQS.VerificationCodes.Commands.ResetVerificationCode;

public class ResetVerificationCodeCommand
{
    [JsonIgnore] 
    public int UserId { get; set; }
    [JsonIgnore]
    public int CurrentUserId { get; set; } 
}