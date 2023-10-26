using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Persistence.Repositories;

public class HallRepository : IHallRepository
{
    private readonly IApplicationDbContext _context;

    public HallRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateAsync(Hall hall, CancellationToken cancellationToken = default)
    {
        if (hall == null)
        {
            throw new ArgumentNullException(nameof(hall));
        }

        await _context.Halls.AddAsync(hall, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return hall.Id;
    }

    public async Task UpdateAsync(Hall hall, CancellationToken cancellationToken = default)
    {
        if (hall == null)
        {
            throw new ArgumentNullException(nameof(hall));
        }
        
        var updatedHall = await _context.Halls.FindAsync(hall.Id);
        if (updatedHall == null)
        {
            throw new EntityNotFoundException(nameof(Hall), hall.Id);
        }

        updatedHall.Name = hall.Name;
        updatedHall.SeatingCapacity = hall.SeatingCapacity;
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        
        var hall = await _context.Halls.FindAsync(id);
        if (hall == null)
        {
            throw new EntityNotFoundException(nameof(Hall), id);
        }

        _context.Halls.Remove(hall);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Hall> GetAsync(HallFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Halls.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Id))
        {
            query = query.Where(h => h.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.VenueId))
        {
            query = query.Where(h => h.VenueId == filter.VenueId);
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(h => h.Name == filter.Name);
        }
        if (filter.SeatingCapacity.HasValue)
        {
            query = query.Where(h => h.SeatingCapacity == filter.SeatingCapacity);
        }

        if (filter is { IncludeSeats: true })
        {
            query = query.Include(h => h.Seats);
        }
        if (filter is { IncludeVenue: true })
        {
            query = query.Include(h => h.Venue);
        }
        if (filter is { IncludeEvent: true })
        {
            query = query.Include(h => h.Event);
        }
        
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Hall>> GetAllAsync(HallFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Halls.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.VenueId))
        {
            query = query.Where(h => h.VenueId == filter.VenueId);
        }
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(h => h.Venue.UserId == filter.UserId);
        }
        if (filter is { IncludeSeats: true })
        {
            query = query.Include(h => h.Seats);
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<Hall> GetQueryable()
    {
        return _context.Halls.AsQueryable();
    }
}