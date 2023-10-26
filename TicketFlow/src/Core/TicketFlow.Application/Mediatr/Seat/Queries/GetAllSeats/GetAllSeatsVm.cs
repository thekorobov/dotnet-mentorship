namespace TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;

public record GetAllSeatsVm
{
    public IList<SeatVm> Seats { get; set; } = new List<SeatVm>();
}