using MediatR;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Queries.QuerySeats;

public record QuerySeatsQuery : IRequest<QuerySeatsVm>
{
    public string? Id { get; set; }
    public string? HallId { get; set; }
    public int? Row { get; set; }
    public int? Number { get; set; }
    public SeatStatus? Status { get; set; }
}