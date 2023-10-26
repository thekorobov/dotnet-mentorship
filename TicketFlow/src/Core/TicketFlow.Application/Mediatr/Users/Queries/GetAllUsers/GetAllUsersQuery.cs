using MediatR;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<GetAllUsersVm>
{
    
}