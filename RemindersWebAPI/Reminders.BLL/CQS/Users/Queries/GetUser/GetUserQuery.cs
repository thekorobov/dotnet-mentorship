using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Reminders.DAL.Entities.Filtres;

namespace Reminders.BLL.CQS.Users.Queries.GetUserById;

public record GetUserQuery
{
    public int? Id { get; set; }
    public string? Email { get; set; }
    [BindNever]
    [JsonIgnore]
    public AuthProviderType AuthProviderType { get; set; }
}