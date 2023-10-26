using Microsoft.EntityFrameworkCore;
using Reminders.DAL.Data;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.DAL.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly ApplicationDbContext _context;

    public ReminderRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateAsync(Reminder? reminder)
    {
        var user = await _context.Users.FindAsync(reminder!.UserId);
        if (user == null) return 0;
        
        reminder.User = user;
        
        await _context.Reminders.AddAsync(reminder);
        await _context.SaveChangesAsync();
        return reminder?.Id ?? 0;
    }

    public async Task UpdateAsync(Reminder? reminder)
    {
        var user = await _context.Users.FindAsync(reminder!.UserId);
        if (user == null) return;
        
        reminder.User = user;
        
        var updatedReminder = await _context.Reminders.FindAsync(reminder!.Id);
        if (updatedReminder != null)
        {
            updatedReminder.Name = reminder.Name;
            updatedReminder.Date = reminder.Date;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Reminder> GetAsync(ReminderFilter reminderFilter)
    {
        return (await _context.Reminders
            .Include(reminder => reminder.User)
            .SingleOrDefaultAsync(reminder => reminder.Id == reminderFilter.Id))!;
    }

    public async Task<IEnumerable<Reminder>> GetAllAsync()
    {
        return await _context.Reminders
            .Include(reminders => reminders.User)
            .ToListAsync();
    }
    
    public IQueryable<Reminder> GetQueryable()
    {
        return _context.Reminders.AsQueryable();
    }

    public async Task DeleteAsync(int id)
    {
        var filter = new ReminderFilter { Id = id };
        var reminder = await GetAsync(filter);
        if (reminder != null)
        {
            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
        }
    }
}