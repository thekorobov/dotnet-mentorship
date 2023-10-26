namespace TicketFlow.Application.Common.Exceptions.Users;

public class PasswordResetFailedException : Exception
{
    public PasswordResetFailedException(string message) : base(message) { }
}