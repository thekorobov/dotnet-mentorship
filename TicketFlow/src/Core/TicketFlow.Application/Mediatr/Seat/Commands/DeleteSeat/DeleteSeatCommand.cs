using MediatR;

namespace TicketFlow.Application.Mediatr.Seat.Commands.DeleteSeat;

public class DeleteSeatCommand : IRequest
{
    public string Id { get; set; }
    public string UserId { get; set; }
}