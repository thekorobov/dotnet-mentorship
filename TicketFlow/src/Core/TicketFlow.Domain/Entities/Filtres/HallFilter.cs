namespace TicketFlow.Domain.Entities.Filters;

public class HallFilter
{
    public string? Id { get; set; }
    public string? VenueId { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public int? SeatingCapacity { get; set; }
    public bool? IncludeSeats { get; set; } = false;
    public bool? IncludeVenue { get; set; } = false;
    public bool? IncludeEvent { get; set; } = false;
}