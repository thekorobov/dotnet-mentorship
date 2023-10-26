using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.Halls;
using TicketFlow.Application.Common.Exceptions.Seats;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Enums;
using TicketFlow.Domain.Enums.Users;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Halls.Commands.CreateHall;

public class CreateHallCommandHandler : IRequestHandler<CreateHallCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHallCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<string> Handle(CreateHallCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Role == UserRole.User.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to create hall.");
        }

        var venue = await _unitOfWork.Venues.GetAsync(new VenueFilter
        {
            Id = command.VenueId, IncludeHalls = true
        }, cancellationToken);

        if (venue.UserId != command.UserId && command.Role != UserRole.Admin.ToString())
        {
            throw new PermissionDeniedException("You don't have permission to create hall for this venue.");
        }
        
        if (venue.SeatingCapacity < command.SeatingCapacity)
        {
            throw new CapacityExceededException("The seating capacity of the hall exceeds the venue's seating capacity.");
        }
        
        var totalHallCapacity = venue.Halls.Sum(h => h.SeatingCapacity);
        var availableSeats = venue.SeatingCapacity - totalHallCapacity;
        
        if (command.SeatingCapacity > availableSeats)
        {
            throw new CapacityExceededException(availableSeats);
        }
        
        var hall = _mapper.Map<CreateHallCommand, Hall>(command);
        
        await _unitOfWork.Halls.CreateAsync(hall, cancellationToken);
        
        var totalSeats = command.RowsCount * command.SeatsPerRow;
        var totalRows = totalSeats / command.SeatsPerRow;
        var lastRowSeats = command.SeatingCapacity - totalSeats;
        
        if (totalSeats > command.SeatingCapacity)
        {
            throw new InvalidSeatConfigurationException("The total number of seats does not match the provided rows and seats per row configuration.");
        }

        var seats = new List<Domain.Entities.Seat>();
        
        for (var i = 1; i <= command.RowsCount; i++)
        {
            for (var j = 1; j <= command.SeatsPerRow; j++)
            {
                var seat = new Domain.Entities.Seat { HallId = hall.Id, Row = i, Number = j, Status = SeatStatus.Available };
                seats.Add(seat);
            }
        }

        if (lastRowSeats > 0)
        {
            for (var j = 1; j <= lastRowSeats; j++)
            {
                var seat = new Domain.Entities.Seat { HallId = hall.Id, Row = totalRows + 1, Number = j, Status = SeatStatus.Available };
                seats.Add(seat);
            }
        }
        
        await _unitOfWork.Seats.CreateRangeAsync(seats, cancellationToken);
        
        return hall.Id;
    }
}