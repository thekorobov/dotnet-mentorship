namespace TicketFlow.Application.Common.Exceptions.Users;

public class RoleAssignmentFailedException : Exception
{
    public RoleAssignmentFailedException(string message) : base(message) { }
}