using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Seat.Commands.DeleteSeat;

public class DeleteSeatCommandHandler : IRequestHandler<DeleteSeatCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSeatCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteSeatCommand command, CancellationToken cancellationToken = default)
    {
        var seat = await _unitOfWork.Seats.GetAsync(new SeatFilter
        {
            Id = command.Id, 
            IncludeHall = true, 
            IncludeVenue = true,
            Status = null
        }, cancellationToken);
        
        if (seat == null)
        {
            throw new EntityNotFoundException(nameof(Domain.Entities.Seat), command.Id);
        }
        
        var user = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.UserId }, cancellationToken);
        if (command.UserId != seat.Hall.Venue.UserId && user.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to delete this seat.");
        }
        
        await _unitOfWork.Seats.DeleteAsync(command.Id, cancellationToken);
        
        return Unit.Value;
    }
}