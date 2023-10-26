using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Halls.Queries.QueryHalls;

public class QueryHallsQueryHandler : IRequestHandler<QueryHallsQuery, QueryHallsVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryHallsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<QueryHallsVm> Handle(QueryHallsQuery query, CancellationToken cancellationToken)
    {
        var halls = _unitOfWork.Halls.GetQueryable();

        halls = halls.Where(v =>
            (!string.IsNullOrEmpty(query.VenueId) && v.VenueId.Contains(query.VenueId)) ||
            (!string.IsNullOrEmpty(query.Id) && v.Id.Contains(query.Id)) ||
            (!string.IsNullOrEmpty(query.Name) && v.Name.ToLower().Contains(query.Name.ToLower())) ||
            (query.SeatingCapacity.HasValue && v.SeatingCapacity == query.SeatingCapacity.Value)
        );

        if (query is {  IncludeSeats: true })
        {
            halls = halls.Include(h => h.Seats);
        }
        
        var result = await halls.ToListAsync(cancellationToken);  

        return new QueryHallsVm { Halls = _mapper.Map<IList<HallVm>>(result) };
    }
}