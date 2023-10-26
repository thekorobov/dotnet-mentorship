using TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;

namespace TicketFlow.Application.Mediatr.Halls.Queries.QueryHalls;

public record QueryHallsVm
{
    public IList<HallVm> Halls { get; set; } = new List<HallVm>();
}