using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;

namespace TicketFlow.Domain.Repositories;

public interface ISeatRepository : IBaseRepository<Seat, SeatFilter>
{
    Task CreateRangeAsync(List<Seat> seats, CancellationToken cancellationToken = default);
}