namespace TicketFlow.Application.Common.Exceptions.Halls;

public class CapacityExceededException : Exception
{
    public CapacityExceededException(string message) : base(message) { }
    
    public CapacityExceededException(int availableSeats)
        : base($"The seating capacity exceeds the available capacity. Only {availableSeats} seats are available.") { }
    
    public CapacityExceededException(int allSeats, int availableSeats)
        : base($"The seating capacity exceeds the available capacity. " +
               $"The hall has a total of {allSeats} seats, " +
               $"but only {availableSeats} seats are available for expansion.") { }
}