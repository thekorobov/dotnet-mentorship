using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;

public class GetAllHallsQueryHandler : IRequestHandler<GetAllHallsQuery, GetAllHallsVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllHallsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetAllHallsVm> Handle(GetAllHallsQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = query.UserId }, cancellationToken);

        if (string.IsNullOrEmpty(query.VenueId))
        {
            var filter = new HallFilter { IncludeSeats = query.IncludeSeats, UserId = user.Id };
            var halls = await _unitOfWork.Halls.GetAllAsync(filter, cancellationToken) ?? new List<Hall>();
            return new GetAllHallsVm { Venues = _mapper.Map<IList<HallVm>>(halls) };
        }

        var existingVenue = await _unitOfWork.Venues.GetAsync(new VenueFilter { Id = query.VenueId }, cancellationToken);

        if (existingVenue == null)
        {
            throw new EntityNotFoundException(nameof(Venue));
        }

        if (user.Id != existingVenue.UserId && user.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to get this halls.");
        }

        var venueFilter = new HallFilter { IncludeSeats = query.IncludeSeats, VenueId = existingVenue.Id };
        var venueHalls = await _unitOfWork.Halls.GetAllAsync(venueFilter, cancellationToken) ?? new List<Hall>();
    
        return new GetAllHallsVm { Venues = _mapper.Map<IList<HallVm>>(venueHalls) };
    }
}