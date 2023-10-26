using Microsoft.EntityFrameworkCore;
using Reminders.DAL.Data;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateAsync(User? user)
    {
        if (user == null)
        {
            return 0;
        }
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user?.Id ?? 0; 
    }

    public async Task UpdateAsync(User? user)
    {
        var updatedUser = await _context.Users.FindAsync(user!.Id);
        if (updatedUser != null)
        {
            updatedUser.Email = user.Email;
            updatedUser.UserName = user.UserName;
            updatedUser.AuthProviderType = user.AuthProviderType;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<User> GetAsync(UserFilter filter)
    {
        var query = _context.Users.AsQueryable();

        if (filter.Id.HasValue)
        {
            query = query.Where(u => u.Id == filter.Id.Value);
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            query = query.Where(u => u.Email == filter.Email);
        }

        return await query.FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public IQueryable<User> GetQueryable()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}