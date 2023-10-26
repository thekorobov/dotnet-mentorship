namespace TicketFlow.Application.Common.Exceptions.VerificationCodes;

public class UserAlreadyVerifiedException : Exception
{
    public UserAlreadyVerifiedException(string message) : base(message) { }
}