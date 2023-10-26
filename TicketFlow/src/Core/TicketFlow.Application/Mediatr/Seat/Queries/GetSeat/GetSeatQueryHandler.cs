using AutoMapper;
using MediatR;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetSeat;

public class GetSeatQueryHandler : IRequestHandler<GetSeatQuery, GetSeatVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSeatQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSeatVm> Handle(GetSeatQuery query, CancellationToken cancellationToken = default)
    {
        var seatFilter = new SeatFilter
        {
            Id = query.Id,
            VenueId = query.HallId,
            Number = query.Number,
            Row = query.Row,
            Status = query.Status,
            UserId = query.UserId
        };

        var seat = await _unitOfWork.Seats.GetAsync(seatFilter, cancellationToken);
        if (seat == null)
        {
            throw new EntityNotFoundException(nameof(Domain.Entities.Seat));
        }
        
        return _mapper.Map<GetSeatVm>(seat);
    }
}