namespace TicketFlow.Domain.Entities;

public class Venue
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int SeatingCapacity { get; set; }
    
    public User User { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<Hall> Halls { get; set; }
}