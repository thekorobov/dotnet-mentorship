using MediatR;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Application.Mediatr.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<string>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }
    public string Forename { get; set; }
    public string Role { get; set; }
    public AuthProviderType AuthProviderType { get; set; }
}