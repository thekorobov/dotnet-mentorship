using TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;

namespace TicketFlow.Application.Mediatr.Seat.Queries.QuerySeats;

public record QuerySeatsVm
{
    public IList<SeatVm> Seats { get; set; } = new List<SeatVm>();
}