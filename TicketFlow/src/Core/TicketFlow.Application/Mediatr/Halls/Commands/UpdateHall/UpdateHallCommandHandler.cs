using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.Halls;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Halls.Commands.UpdateHall;

public class UpdateHallCommandHandler : IRequestHandler<UpdateHallCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateHallCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateHallCommand command, CancellationToken cancellationToken = default)
    {
        var hall = await _unitOfWork.Halls.GetAsync(new HallFilter { Id = command.Id }, cancellationToken);
        if (hall == null)
        {
            throw new EntityNotFoundException(nameof(Hall), command.Id);
        }
        
        var venue = await _unitOfWork.Venues.GetAsync(new VenueFilter { Id = hall.VenueId, IncludeHalls = true}, cancellationToken);
        if (venue == null)
        {
            throw new EntityNotFoundException(nameof(Venue), command.Id);
        }
        
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        if (venue.UserId != command.UserId && user.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to update this hall.");
        }
        
        if (venue.SeatingCapacity < command.SeatingCapacity)
        {
            throw new CapacityExceededException("The seating capacity of the hall exceeds the venue's seating capacity.");
        }
        
        var totalHallCapacity = venue.Halls.Sum(h => h.SeatingCapacity);
        var availableSeats = venue.SeatingCapacity - totalHallCapacity + hall.SeatingCapacity;
        
        if (command.SeatingCapacity > availableSeats)
        {
            throw new CapacityExceededException(venue.SeatingCapacity, availableSeats);
        }

        var updatedHall = new Hall();
        _mapper.Map(command, updatedHall);
        
        await _unitOfWork.Halls.UpdateAsync(updatedHall, cancellationToken);
        
        return Unit.Value;
    }
}