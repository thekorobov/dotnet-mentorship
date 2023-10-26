namespace TicketFlow.Domain.Entities.Filters;

public class VerificationCodeFilter
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? VerificationToken { get; set; }
}