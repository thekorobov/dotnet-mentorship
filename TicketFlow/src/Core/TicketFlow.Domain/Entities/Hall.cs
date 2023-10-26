namespace TicketFlow.Domain.Entities;

public class Hall
{
    public string Id { get; set; }
    public string VenueId { get; set; }
    public string Name { get; set; }
    public int SeatingCapacity { get; set; }
    
    public Venue Venue { get; set; }
    public Event Event { get; set; }
    public ICollection<Seat> Seats { get; set; }
}