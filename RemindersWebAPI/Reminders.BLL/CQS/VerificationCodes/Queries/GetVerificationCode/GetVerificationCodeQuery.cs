namespace Reminders.BLL.CQS.VerificationCodes.Queries.GetVerificationCode;

public class GetVerificationCodeQuery
{
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string? VerificationToken { get; set; }
}