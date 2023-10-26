using MediatR;

namespace TicketFlow.Application.Mediatr.Halls.Commands.DeleteHall;

public record DeleteHallCommand : IRequest
{
    public string Id { get; set; }
    public string UserId { get; set; }
}