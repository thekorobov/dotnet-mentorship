using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Venues.Queries.GetVenue;

public class GetVenueQueryHandler: IRequestHandler<GetVenueQuery, GetVenueVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVenueQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetVenueVm> Handle(GetVenueQuery query, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = query.UserId }, cancellationToken);
        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User), query.UserId);
        }
        
        var filter = new VenueFilter 
        {
            Id = query.Id,
            Name = query.Name,
            Address = query.Address,
            SeatingCapacity = query.SeatingCapacity,
            IncludeHalls = query.IncludeHalls,
            IncludeSeats = query.IncludeSeats
        };

        var venue = await _unitOfWork.Venues.GetAsync(filter, cancellationToken);
        
        if (venue == null)
        {
            throw new EntityNotFoundException(nameof(Venue));
        }
        
        if (existingUser.Id != venue.UserId && existingUser.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to get this venue.");
        }

        return _mapper.Map<GetVenueVm>(venue);
    }
}