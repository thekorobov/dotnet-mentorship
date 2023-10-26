using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.Seats;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Seat.Commands.UpdateSeat;

public class UpdateSeatCommandHandler : IRequestHandler<UpdateSeatCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSeatCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateSeatCommand command, CancellationToken cancellationToken = default)
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
        if (seat.Hall.Venue.UserId != command.UserId && user.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to update this seat.");
        }
        
        var hall = await _unitOfWork.Halls.GetAsync(new HallFilter
        {
            Id = command.HallId, 
            IncludeSeats = true
        }, cancellationToken);
        
        var seatWithSameRowAndNumber = hall.Seats.FirstOrDefault(s => s.Row == command.Row && s.Number == command.Number);

        if (seatWithSameRowAndNumber != null && seatWithSameRowAndNumber.Id != command.Id)
        {
            throw new SeatAlreadyExistsException(command.Row, command.Number);
        }
        
        var updatedSeat = new Domain.Entities.Seat();
        _mapper.Map(command, updatedSeat);
        
        await _unitOfWork.Seats.UpdateAsync(updatedSeat, cancellationToken);
        
        return Unit.Value;
    }
}