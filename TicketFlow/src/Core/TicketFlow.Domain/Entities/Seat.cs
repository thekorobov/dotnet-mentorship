using TicketFlow.Domain.Enums;

namespace TicketFlow.Domain.Entities;

public class Seat
{
    public string Id { get; set; }
    public string HallId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public SeatStatus Status { get; set; }
    
    public Hall Hall { get; set; }
    public Ticket Ticket { get; set; }
}