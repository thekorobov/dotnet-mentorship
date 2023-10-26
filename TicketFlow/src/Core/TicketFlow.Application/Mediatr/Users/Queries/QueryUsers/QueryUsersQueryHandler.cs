using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Mediatr.Users.Queries.GetAllUsers;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Queries.QueryUsers;

public class QueryUsersQueryHandler : IRequestHandler<QueryUsersQuery, QueryUsersVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QueryUsersVm> Handle(QueryUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Users.GetQueryable();
    
        query = query.Where(u =>
            (!string.IsNullOrEmpty(request.Id) && u.Id == request.Id) ||
            (!string.IsNullOrEmpty(request.Email) && u.Email!.Contains(request.Email)) ||
            (!string.IsNullOrEmpty(request.UserName) && u.UserName!.ToLower().Contains(request.UserName.ToLower())) ||
            (!string.IsNullOrEmpty(request.Surname) && u.Surname.ToLower().Contains(request.Surname.ToLower())) ||
            (!string.IsNullOrEmpty(request.Forename) && u.Forename.ToLower().Contains(request.Forename.ToLower())) ||
            (!string.IsNullOrEmpty(request.Role) && u.Role.ToLower().Contains(request.Role.ToLower()))
        );

        var users = await query.ToListAsync(cancellationToken);  

        return new QueryUsersVm { Users = _mapper.Map<IList<UserVm>>(users) };
    }
}