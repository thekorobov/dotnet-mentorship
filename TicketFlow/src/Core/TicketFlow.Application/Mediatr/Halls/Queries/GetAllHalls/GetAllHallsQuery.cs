using MediatR;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;

public record GetAllHallsQuery : IRequest<GetAllHallsVm>
{
    public string? VenueId { get; set; }
    public string? UserId { get; set; }
    public bool? IncludeSeats { get; set; } = false;
}