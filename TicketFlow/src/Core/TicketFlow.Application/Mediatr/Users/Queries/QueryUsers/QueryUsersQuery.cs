using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Application.Mediatr.Users.Queries.QueryUsers;

public record QueryUsersQuery : IRequest<QueryUsersVm>
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Role { get; set; }
    [BindNever]
    [JsonIgnore]
    public AuthProviderType AuthProviderType { get; set; }
}