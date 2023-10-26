using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetHall;

public class GetHallQueryHandler : IRequestHandler<GetHallQuery, GetHallVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetHallQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<GetHallVm> Handle(GetHallQuery query, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = query.UserId }, cancellationToken);
        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User), query.UserId);
        }
        
        var hallFilter = new HallFilter()
        {
            Id = query.Id,
            VenueId = query.VenueId,
            Name = query.Name,
            SeatingCapacity = query.SeatingCapacity,
            IncludeSeats = query.IncludeSeats
        };

        var hall = await _unitOfWork.Halls.GetAsync(hallFilter, cancellationToken);
        
        if (hall == null)
        {
            throw new EntityNotFoundException(nameof(Hall));
        }

        var venue = await _unitOfWork.Venues.GetAsync(new VenueFilter { Id = hall.VenueId }, cancellationToken);
        
        if (existingUser.Id != venue.UserId && existingUser.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to get this hall.");
        }
        
        return _mapper.Map<GetHallVm>(hall);
    }
}