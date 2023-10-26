using Reminders.DAL.Entities;

namespace Reminders.BLL.DTO;

public class VerificationCodeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }  
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public virtual User User { set; get; }
}