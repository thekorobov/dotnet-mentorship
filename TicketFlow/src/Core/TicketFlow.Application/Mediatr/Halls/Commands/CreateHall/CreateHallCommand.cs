using MediatR;

namespace TicketFlow.Application.Mediatr.Halls.Commands.CreateHall;

public record CreateHallCommand : IRequest<string>
{
    public string VenueId { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public int SeatingCapacity { get; set; }
    public int RowsCount { get; set; }
    public int SeatsPerRow { get; set; }
    public string Role { get; set; }
}