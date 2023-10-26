namespace TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;

public class GetAllVenuesVm 
{
    public IList<VenueVm> Venues { get; set; } = new List<VenueVm>();
}