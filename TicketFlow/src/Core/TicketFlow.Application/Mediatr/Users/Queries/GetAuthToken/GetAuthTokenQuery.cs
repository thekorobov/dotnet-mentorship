using System.Text.Json.Serialization;
using MediatR;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAuthToken;

public record GetAuthTokenQuery : IRequest<GetAuthTokenVm>
{
    public string Email { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public AuthProviderType AuthProviderType { get; set; }
}