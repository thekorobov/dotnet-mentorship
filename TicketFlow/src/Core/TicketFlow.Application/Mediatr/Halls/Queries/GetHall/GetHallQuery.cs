using MediatR;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetHall;

public record GetHallQuery : IRequest<GetHallVm>
{
    public string? Id { get; set; }
    public string? VenueId { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public int? SeatingCapacity { get; set; }
    public bool? IncludeSeats { get; set; } = false;
}