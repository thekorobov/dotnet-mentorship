using AutoMapper;
using MediatR;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetAllUsersVm> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.Users.GetAllAsync(new UserFilter(), cancellationToken) ?? new List<User>();
        
        return new GetAllUsersVm { Users = _mapper.Map<IList<UserVm>>(users) };
    }
}