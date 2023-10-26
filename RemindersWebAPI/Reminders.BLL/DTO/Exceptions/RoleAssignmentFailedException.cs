namespace Reminders.BLL.DTO.Exceptions;

public class RoleAssignmentFailedException : Exception
{
    public RoleAssignmentFailedException(string message) : base(message) { }
}