using AutoMapper;
using MediatR;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;

public class GetAllSeatsQueryHandler : IRequestHandler<GetAllSeatsQuery, GetAllSeatsVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSeatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetAllSeatsVm> Handle(GetAllSeatsQuery query, CancellationToken cancellationToken = default)
    {
        var filter = new SeatFilter 
        {
            VenueId = query.VenueId, 
            HallId = query.HallId,
            UserId = query.UserId,
            Status = query.Status
        };
        
        var seats = await _unitOfWork.Seats.GetAllAsync(filter, cancellationToken) ?? new List<Domain.Entities.Seat>();
        
        return new GetAllSeatsVm { Seats = _mapper.Map<IList<SeatVm>>(seats) };
    }
}