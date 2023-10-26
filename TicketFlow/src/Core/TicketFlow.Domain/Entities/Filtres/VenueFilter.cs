namespace TicketFlow.Domain.Entities.Filters;

public class VenueFilter
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public int? SeatingCapacity { get; set; }
    public bool? IncludeHalls { get; set; } = false;
    public bool? IncludeSeats { get; set; } = false;
}