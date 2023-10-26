namespace TicketFlow.Domain.Entities;

public class Event
{
    public string Id { get; set; }
    public string VenueId { get; set; }
    public string HallId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    
    public Venue Venue { get; set; }
    public Hall Hall { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}