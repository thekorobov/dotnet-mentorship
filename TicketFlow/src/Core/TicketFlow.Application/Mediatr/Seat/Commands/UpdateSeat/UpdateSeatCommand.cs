using MediatR;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Commands.UpdateSeat;

public class UpdateSeatCommand : IRequest
{
    public string Id { get; set; }
    public string HallId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public SeatStatus Status { get; set; }
    
    public string UserId { get; set; }
    public string Role { get; set; }
}