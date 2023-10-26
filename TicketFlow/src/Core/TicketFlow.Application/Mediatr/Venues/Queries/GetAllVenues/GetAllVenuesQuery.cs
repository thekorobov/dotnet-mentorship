using MediatR;

namespace TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;

public class GetAllVenuesQuery : IRequest<GetAllVenuesVm>
{
    public string? UserId { get; set; }
    public bool? IncludeHalls { get; set; } = false;
    public bool? IncludeSeats { get; set; } = false;
}