using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetUserVm> Handle(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        var userFilter = new UserFilter
        {
            Id = query.Id,
            Email = query.Email,
            UserName = query.UserName
        };

        var user = await _unitOfWork.Users.GetAsync(userFilter, cancellationToken);
    
        if (user == null)
        {
            var identifier = !string.IsNullOrEmpty(query.Id) ? query.Id : query.Email;
            throw new EntityNotFoundException(nameof(User), identifier);
        }

        return _mapper.Map<GetUserVm>(user);
    }
}