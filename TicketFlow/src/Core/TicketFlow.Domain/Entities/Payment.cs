namespace TicketFlow.Domain.Entities;

public class Payment
{
    public string Id { get; set; }
    public string TicketId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    
    public Ticket Ticket { get; set; }
}