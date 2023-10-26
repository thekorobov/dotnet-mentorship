using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Seat.Queries.QuerySeats;

public class QuerySeatsQueryHandler : IRequestHandler<QuerySeatsQuery, QuerySeatsVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuerySeatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<QuerySeatsVm> Handle(QuerySeatsQuery query, CancellationToken cancellationToken)
    {
        var seats = _unitOfWork.Seats.GetQueryable();

        seats = seats.Where(v =>
            (!string.IsNullOrEmpty(query.HallId) && v.HallId.Contains(query.HallId)) ||
            (!string.IsNullOrEmpty(query.Id) && v.Id.Contains(query.Id)) ||
            (query.Row.HasValue && v.Row == query.Row.Value) ||
            (query.Number.HasValue && v.Number == query.Number.Value) ||
            (query.Status.HasValue && v.Status == query.Status.Value)
        );
        
        var result = await seats.ToListAsync(cancellationToken);  

        return new QuerySeatsVm { Seats = _mapper.Map<IList<SeatVm>>(result) };
    }
}