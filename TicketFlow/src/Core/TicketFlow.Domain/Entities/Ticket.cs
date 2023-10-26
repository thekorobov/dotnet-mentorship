using TicketFlow.Domain.Enums;

namespace TicketFlow.Domain.Entities;

public class Ticket
{
    public string Id { get; set; }
    public string EventId { get; set; }
    public string SeatId { get; set; }
    public string UserId { get; set; }
    public decimal Price { get; set; }
    public TicketStatus Status { get; set; }
    
    public Payment Payment { get; set; }
    public Event Event { get; set; }
    public Seat Seat { get; set; }
    public User User { get; set; }
}