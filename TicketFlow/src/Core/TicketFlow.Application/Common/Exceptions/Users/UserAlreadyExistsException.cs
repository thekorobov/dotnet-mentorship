namespace TicketFlow.Application.Common.Exceptions.Users;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message) { }
}