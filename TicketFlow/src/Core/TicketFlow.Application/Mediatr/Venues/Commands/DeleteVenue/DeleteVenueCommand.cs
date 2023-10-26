using MediatR;

namespace TicketFlow.Application.Mediatr.Venues.Commands.DeleteVenue;

public record DeleteVenueCommand : IRequest
{
    public string Id { get; set; }
    public string UserId { get; set; }
}