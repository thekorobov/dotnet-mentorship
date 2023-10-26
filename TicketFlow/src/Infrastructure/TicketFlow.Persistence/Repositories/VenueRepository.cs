using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Persistence.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly IApplicationDbContext _context;

    public VenueRepository(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<string> CreateAsync(Venue venue, CancellationToken cancellationToken = default)
    {
        if (venue == null)
        {
            throw new ArgumentNullException(nameof(venue));
        }

        await _context.Venues.AddAsync(venue, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return venue.Id;
    }

    public async Task UpdateAsync(Venue venue, CancellationToken cancellationToken = default)
    {
        if (venue == null)
        {
            throw new ArgumentNullException(nameof(venue));
        }
        
        var updatedVenue = await _context.Venues.FindAsync(venue!.Id);
        if (updatedVenue == null)
        {
            throw new EntityNotFoundException(nameof(Venue), venue.Id);
        }

        updatedVenue.Name = venue.Name;
        updatedVenue.Address = venue.Address;
        updatedVenue.SeatingCapacity = venue.SeatingCapacity;
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null)
        {
            throw new EntityNotFoundException(nameof(Venue), id);
        }

        _context.Venues.Remove(venue);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Venue> GetAsync(VenueFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Venues.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Id))
        {
            query = query.Where(v => v.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(v => v.UserId == filter.UserId);
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(v => v.Name == filter.Name);
        }
        if (!string.IsNullOrEmpty(filter.Address))
        {
            query = query.Where(v => v.Address == filter.Address);
        }
        if (filter.SeatingCapacity.HasValue)
        {
            query = query.Where(v => v.SeatingCapacity == filter.SeatingCapacity);
        }
        
        if (filter is { IncludeHalls: true, IncludeSeats: true })
        {
            query = query.Include(v => v.Halls).ThenInclude(h => h.Seats);
        }
        if (filter is { IncludeHalls: true, IncludeSeats: false })
        {
            query = query.Include(v => v.Halls);
        }
        
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Venue>> GetAllAsync(VenueFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Venues.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(v => v.UserId == filter.UserId);
        }
        if (filter is { IncludeHalls: true, IncludeSeats: true })
        {
            query = query.Include(v => v.Halls).ThenInclude(h => h.Seats);
        }
        if (filter is { IncludeHalls: true, IncludeSeats: false })
        {
            query = query.Include(v => v.Halls);
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<Venue> GetQueryable()
    {
        return _context.Venues.AsQueryable();
    }
}