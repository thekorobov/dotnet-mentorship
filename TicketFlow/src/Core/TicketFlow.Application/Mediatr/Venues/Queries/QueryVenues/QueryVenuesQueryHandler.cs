using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Venues.Queries.QueryVenues;

public class QueryVenuesQueryHandler : IRequestHandler<QueryVenuesQuery, QueryVenuesVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryVenuesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QueryVenuesVm> Handle(QueryVenuesQuery query, CancellationToken cancellationToken)
    {
        var venues = _unitOfWork.Venues.GetQueryable();

        venues = venues.Where(v =>
            (!string.IsNullOrEmpty(query.UserId) && v.UserId.Contains(query.UserId)) ||
            (!string.IsNullOrEmpty(query.Id) && v.Id.Contains(query.Id)) ||
            (!string.IsNullOrEmpty(query.Name) && v.Name.ToLower().Contains(query.Name.ToLower())) ||
            (!string.IsNullOrEmpty(query.Address) && v.Address.ToLower().Contains(query.Address.ToLower())) ||
            (query.SeatingCapacity.HasValue && v.SeatingCapacity == query.SeatingCapacity.Value)
        );

        if (query is { IncludeHalls: true, IncludeSeats: true })
        {
            venues = venues.Include(v => v.Halls).ThenInclude(h => h.Seats);
        }
        if (query is { IncludeHalls: true, IncludeSeats: false })
        {
            venues = venues.Include(v => v.Halls);
        }
        
        var result = await venues.ToListAsync(cancellationToken);  

        return new QueryVenuesVm { Venues = _mapper.Map<IList<VenueVm>>(result) };
    }
}