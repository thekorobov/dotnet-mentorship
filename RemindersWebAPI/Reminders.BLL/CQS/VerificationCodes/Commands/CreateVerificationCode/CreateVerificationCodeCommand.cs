using System.Text.Json.Serialization;

namespace Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;

public class CreateVerificationCodeCommand
{
    [JsonIgnore] 
    public int UserId { get; set; }
}