namespace Reminders.BLL.CQS.Users.Commands.DeleteUser;

public record DeleteUserCommand
{
    public int Id { get; set; }
    public int CurrentUserId { get; set; }
}