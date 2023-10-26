using System.Text.Json.Serialization;
using Reminders.DAL.Entities.Filtres;

namespace Reminders.BLL.CQS.Users.Commands.CreateUser;

public record CreateUserCommand
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public AuthProviderType AuthProviderType { get; set; }
}