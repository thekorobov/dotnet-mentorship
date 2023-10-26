using Microsoft.AspNetCore.Identity;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Domain.Entities;

public class User : IdentityUser<string>  
{
    public string Surname { get; set; }
    public string Forename { get; set; }
    public string Role { get; set; }
    public AuthProviderType AuthProviderType { get; set; }
    
    public VerificationCode VerificationCode { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<Venue> Venues { get; set; }
}