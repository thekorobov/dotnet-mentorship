using MediatR;

namespace TicketFlow.Application.Mediatr.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest
{
    public string Id { get; set; }
    public string CurrentUserId { get; set; } 
    
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }
    public string Forename { get; set; }
}