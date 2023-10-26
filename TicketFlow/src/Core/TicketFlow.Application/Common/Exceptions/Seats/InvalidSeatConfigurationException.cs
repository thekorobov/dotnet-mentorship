namespace TicketFlow.Application.Common.Exceptions.Seats;

public class InvalidSeatConfigurationException : Exception
{
    public InvalidSeatConfigurationException(string message) : base(message) { }
}