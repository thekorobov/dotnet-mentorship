using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Persistence.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly IApplicationDbContext _context;

    public SeatRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateAsync(Seat seat, CancellationToken cancellationToken = default)
    {
        if (seat == null)
        {
            throw new ArgumentNullException(nameof(seat));
        }

        await _context.Seats.AddAsync(seat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return seat.Id;
    }

    public async Task CreateRangeAsync(List<Seat> seats, CancellationToken cancellationToken = default)
    {
        if (seats == null)
        {
            throw new ArgumentNullException(nameof(seats));
        }

        await _context.Seats.AddRangeAsync(seats, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(Seat seat, CancellationToken cancellationToken = default)
    {
        if (seat == null)
        {
            throw new ArgumentNullException(nameof(seat));
        }
        
        var updatedSeat = await _context.Seats.FindAsync(seat.Id, cancellationToken);
        if (updatedSeat == null)
        {
            throw new EntityNotFoundException(nameof(Seat), seat.Id);
        }

        updatedSeat.Number = seat.Number;
        updatedSeat.Row = seat.Row;
        updatedSeat.Status = seat.Status;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        
        var seat = await _context.Seats.FindAsync(id);
        if (seat == null)
        {
            throw new EntityNotFoundException(nameof(Seat), id);
        }

        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Seat> GetAsync(SeatFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Seats.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Id))
        {
            query = query.Where(s => s.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.HallId))
        {
            query = query.Where(s => s.HallId == filter.HallId);
        }
        if (filter.Row.HasValue)
        {
            query = query.Where(s => s.Row == filter.Row);
        }
        if (filter.Number.HasValue)
        {
            query = query.Where(s => s.Number == filter.Number);
        }
        if (filter.Status.HasValue)
        {
            query = query.Where(s => s.Status == filter.Status);
        }
        
        if (filter is { IncludeHall: true, IncludeVenue: true })
        {
            query = query.Include(h => h.Hall).ThenInclude(h => h.Venue);
        }
        if (filter is { IncludeHall: true })
        {
            query = query.Include(h => h.Hall);
        }
        
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Seat>> GetAllAsync(SeatFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Seats
            .Include(s => s.Hall)
            .ThenInclude(h => h.Venue)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.HallId))
        {
            query = query.Where(s => s.HallId == filter.HallId);
        }
        if (!string.IsNullOrEmpty(filter.VenueId))
        {
            query = query.Where(s => s.Hall.VenueId == filter.VenueId);
        }
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(s => s.Hall.Venue.UserId == filter.UserId);
        }
        if (filter.Status.HasValue)
        {
            query = query.Where(s => s.Status == filter.Status);
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<Seat> GetQueryable()
    {
        return _context.Seats.AsQueryable();
    }
}