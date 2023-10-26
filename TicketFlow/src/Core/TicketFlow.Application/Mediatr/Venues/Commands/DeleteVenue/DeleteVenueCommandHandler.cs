using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Venues.Commands.DeleteVenue;

public class DeleteVenueCommandHandler : IRequestHandler<DeleteVenueCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVenueCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteVenueCommand command, CancellationToken cancellationToken = default)
    {
        var existingVenue = await _unitOfWork.Venues.GetAsync(new VenueFilter { Id = command.Id }, cancellationToken);
        if (existingVenue == null)
        {
            throw new EntityNotFoundException(nameof(Venue), command.Id);
        }
        
        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        if (command.UserId != existingVenue.UserId && existingUser.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to delete this venue.");
        }
        
        await _unitOfWork.Venues.DeleteAsync(command.Id, cancellationToken);
        
        return Unit.Value;
    }
}