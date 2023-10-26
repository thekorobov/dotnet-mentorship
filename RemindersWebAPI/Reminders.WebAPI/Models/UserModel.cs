using Reminders.DAL.Entities.Filtres;

namespace Reminders.WebAPI.Models;

public class UserModel
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Role { set; get; }
    public AuthProviderType AuthProviderType { get; set; }
}