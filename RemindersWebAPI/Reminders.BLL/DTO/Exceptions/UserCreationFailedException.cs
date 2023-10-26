namespace Reminders.BLL.DTO.Exceptions;

public class UserCreationFailedException : Exception
{
    public UserCreationFailedException(string message) : base(message) { }
}