using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.Halls;
using TicketFlow.Application.Common.Exceptions.Seats;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Seat.Commands.CreateSeat;

public class CreateSeatCommandHandler : IRequestHandler<CreateSeatCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSeatCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateSeatCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Role == UserRole.User.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to create seat.");
        }

        var hall = await _unitOfWork.Halls.GetAsync(new HallFilter
        {
            Id = command.HallId, 
            IncludeSeats = true, 
            IncludeVenue = true
        }, cancellationToken);
        
        if (hall.Venue.UserId != command.UserId && command.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to create seat for this hall.");
        }

        var totalSeatsInHall = hall.SeatingCapacity;
        var occupiedSeatsInHall = hall.Seats.Count; 

        if (occupiedSeatsInHall >= totalSeatsInHall)
        {
            throw new CapacityExceededException("The hall is already full. No seats are available.");
        }

        var seatWithSameRowAndNumber = hall.Seats.FirstOrDefault(s => s.Row == command.Row && s.Number == command.Number);

        if (seatWithSameRowAndNumber != null)
        {
            throw new SeatAlreadyExistsException(command.Row, command.Number);
        }
        
        var seat = _mapper.Map<CreateSeatCommand, Domain.Entities.Seat>(command);
        await _unitOfWork.Seats.CreateAsync(seat, cancellationToken);

        return seat.Id;
    }
}