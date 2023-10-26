using TicketFlow.Domain.Enums;

namespace TicketFlow.Domain.Entities.Filters;

public class SeatFilter
{
    public string? Id { get; set; }
    public string? HallId { get; set; }
    public int? Row { get; set; }
    public int? Number { get; set; }
    public SeatStatus? Status { get; set; } = SeatStatus.Available;
    public string? VenueId { get; set; }
    public string? UserId { get; set; }
    public bool? IncludeHall { get; set; } = false;
    public bool? IncludeVenue { get; set; } = false;
}