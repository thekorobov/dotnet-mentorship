using MediatR;

namespace TicketFlow.Application.Mediatr.Users.Commands.DeleteUser;

public record DeleteUserCommand : IRequest
{
    public string Id { get; set; }
    public string CurrentUserId { get; set; }
}