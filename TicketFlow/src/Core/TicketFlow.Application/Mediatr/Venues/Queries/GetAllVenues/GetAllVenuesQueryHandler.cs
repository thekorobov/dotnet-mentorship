using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;

public class GetAllVenuesQueryHandler : IRequestHandler<GetAllVenuesQuery, GetAllVenuesVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllVenuesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetAllVenuesVm> Handle(GetAllVenuesQuery query, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = query.UserId }, cancellationToken);
        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User), query.UserId);
        }
        
        var filter = new VenueFilter 
        {
            IncludeHalls = query.IncludeHalls, 
            IncludeSeats = query.IncludeSeats 
        };

        if (existingUser.Role != UserRole.Admin.ToString())
        {
            filter.UserId = query.UserId;
        }
        
        var venues = await _unitOfWork.Venues.GetAllAsync(filter, cancellationToken) ?? new List<Venue>();
        
        return new GetAllVenuesVm { Venues = _mapper.Map<IList<VenueVm>>(venues) };
    }
}