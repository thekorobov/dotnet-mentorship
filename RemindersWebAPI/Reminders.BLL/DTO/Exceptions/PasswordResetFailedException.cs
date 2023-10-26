namespace Reminders.BLL.DTO.Exceptions;

public class PasswordResetFailedException : Exception
{
    public PasswordResetFailedException(string message) : base(message) { }
}