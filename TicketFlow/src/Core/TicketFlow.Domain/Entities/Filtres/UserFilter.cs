namespace TicketFlow.Domain.Entities.Filters;

public class UserFilter
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
}