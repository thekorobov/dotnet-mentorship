using MediatR;

namespace TicketFlow.Application.Mediatr.Venues.Commands.CreateVenue;

public record CreateVenueCommand : IRequest<string>
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int SeatingCapacity { get; set; }
    public string Role { get; set; }
}