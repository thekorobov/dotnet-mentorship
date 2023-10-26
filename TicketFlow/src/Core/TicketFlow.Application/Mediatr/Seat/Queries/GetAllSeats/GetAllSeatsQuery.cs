using MediatR;
using TicketFlow.Domain.Enums;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;

public record GetAllSeatsQuery : IRequest<GetAllSeatsVm>
{
    public string? VenueId { get; set; }
    public string? UserId { get; set; }
    public string? HallId { get; set; }
    public SeatStatus? Status { get; set; }
}