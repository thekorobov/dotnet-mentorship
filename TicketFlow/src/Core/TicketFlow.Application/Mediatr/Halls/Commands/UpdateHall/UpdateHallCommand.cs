using MediatR;

namespace TicketFlow.Application.Mediatr.Halls.Commands.UpdateHall;

public record UpdateHallCommand : IRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int SeatingCapacity { get; set; }
    
    public string UserId { get; set; }
    public string Role { get; set; }
}