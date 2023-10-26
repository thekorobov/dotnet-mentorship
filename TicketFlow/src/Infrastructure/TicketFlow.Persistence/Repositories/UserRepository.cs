using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Interfaces;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var updatedUser = await _context.Users.FindAsync(user.Id);
        if (updatedUser == null)
        {
            throw new EntityNotFoundException(nameof(User), user.Id);
        }
        
        _mapper.Map(user, updatedUser);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new EntityNotFoundException(nameof(User), id);
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> GetAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.Id))
        {
            query = query.Where(u => u.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.Email))
        {
            query = query.Where(u => u.Email == filter.Email);
        }
        if (!string.IsNullOrEmpty(filter.UserName))
        {
            query = query.Where(u => u.UserName == filter.UserName);
        }
        if (!string.IsNullOrEmpty(filter.Surname))
        {
            query = query.Where(u => u.Surname == filter.Surname);
        }
        if (!string.IsNullOrEmpty(filter.Forename))
        {
            query = query.Where(u => u.Forename == filter.Forename);
        }
        if (!string.IsNullOrEmpty(filter.Role))
        {
            query = query.Where(u => u.Role == filter.Role);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public IQueryable<User> GetQueryable()
    {
        return _context.Users.AsQueryable();
    }
}