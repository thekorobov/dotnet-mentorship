namespace Reminders.BLL.DTO.Exceptions;

public class UserAlreadyVerifiedException : Exception
{
    public UserAlreadyVerifiedException(string message) : base(message) { }
}