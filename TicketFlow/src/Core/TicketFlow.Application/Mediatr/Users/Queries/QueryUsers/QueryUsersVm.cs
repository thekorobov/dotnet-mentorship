using TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;

namespace TicketFlow.Application.Mediatr.Users.Queries.QueryUsers;

public record QueryUsersVm
{
    public IList<UserVm> Users { get; set; } = new List<UserVm>();
}