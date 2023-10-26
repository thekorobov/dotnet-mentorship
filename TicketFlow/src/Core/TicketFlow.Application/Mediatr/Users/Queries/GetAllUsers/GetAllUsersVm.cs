namespace TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;

public record GetAllUsersVm
{
    public IList<UserVm> Users { get; set; } = new List<UserVm>();
}