using System.Text.Json.Serialization;
using Reminders.DAL.Entities.Filtres;

namespace Reminders.BLL.CQS.Users.Queries.GetAuthToken;

public record GetAuthTokenQuery
{
    public string Email { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public AuthProviderType AuthProviderType { get; set; }
}