namespace TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;

public record GetAllHallsVm
{
    public IList<HallVm> Venues { get; set; } = new List<HallVm>();
}