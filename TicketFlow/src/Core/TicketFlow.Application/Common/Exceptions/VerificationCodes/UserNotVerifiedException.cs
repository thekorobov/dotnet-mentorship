namespace TicketFlow.Application.Common.Exceptions.VerificationCodes;

public class UserNotVerifiedException : Exception
{
    public UserNotVerifiedException(string message) : base(message) { }
}