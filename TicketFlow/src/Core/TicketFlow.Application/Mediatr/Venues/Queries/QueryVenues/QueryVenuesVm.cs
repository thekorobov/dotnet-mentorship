using TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;

namespace TicketFlow.Application.Mediatr.Venues.Queries.QueryVenues;

public record QueryVenuesVm
{
    public IList<VenueVm> Venues { get; set; } = new List<VenueVm>();
}