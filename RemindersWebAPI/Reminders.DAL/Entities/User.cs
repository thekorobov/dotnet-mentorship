using Microsoft.AspNetCore.Identity;
using Reminders.DAL.Entities.Filtres;

namespace Reminders.DAL.Entities;

public class User : IdentityUser<int>  
{
    public string? Role { get; set; }
    public AuthProviderType AuthProviderType { get; set; }
    public ICollection<Reminder> Reminders { get; set; }
}