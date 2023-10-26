namespace TicketFlow.Domain.Entities;

public class VerificationCode
{
    public string Id { get; set; }
    public string UserId { get; set; }  
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public User User { set; get; }
}