namespace Reminders.DAL.Entities.Filtres;

public class VerificationCodeFilter
{
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string? VerificationToken { get; set; }
}