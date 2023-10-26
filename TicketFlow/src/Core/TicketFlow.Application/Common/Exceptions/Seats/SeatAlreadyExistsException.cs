namespace TicketFlow.Application.Common.Exceptions.Seats;

public class SeatAlreadyExistsException : Exception
{
    public SeatAlreadyExistsException(int row, int number)
        : base($"Seat with row {row} and number {number} already exists in the hall.") { }
}