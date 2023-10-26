using MediatR;

namespace TicketFlow.Application.Mediatr.Halls.Queries.QueryHalls;

public record QueryHallsQuery : IRequest<QueryHallsVm>
{
    public string? Id { get; set; }
    public string? VenueId { get; set; }
    public string? Name { get; set; }
    public int? SeatingCapacity { get; set; }
    public bool? IncludeSeats { get; set; } = false;
}