using MediatR;

namespace TicketFlow.Application.Mediatr.Venues.Commands.UpdateVenue;

public record UpdateVenueCommand : IRequest
{
    public string Id { get; set; }
    public string UserId { get; set; } 
    public string Name { get; set; }
    public string Address { get; set; }
    public int SeatingCapacity { get; set; }
}