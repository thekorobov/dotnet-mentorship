using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Halls.Commands.DeleteHall;

public class DeleteHallCommandHandler : IRequestHandler<DeleteHallCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHallCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteHallCommand command, CancellationToken cancellationToken = default)
    {
        var hall = await _unitOfWork.Halls.GetAsync(new HallFilter { Id = command.Id, IncludeVenue = true}, cancellationToken);
        if (hall == null)
        {
            throw new EntityNotFoundException(nameof(Hall), command.Id);
        }
        
        if (hall.Venue == null)
        {
            throw new EntityNotFoundException(nameof(Venue), hall.VenueId);
        }
        
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        if (command.UserId != hall.Venue.UserId && user.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to delete this hall.");
        }
        
        await _unitOfWork.Halls.DeleteAsync(command.Id, cancellationToken);
        
        return Unit.Value;
    }
}