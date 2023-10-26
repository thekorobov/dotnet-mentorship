namespace TicketFlow.Application.Common.Exceptions.Users;

public class UserCreationFailedException : Exception
{
    public UserCreationFailedException(string message) : base(message) { }
}