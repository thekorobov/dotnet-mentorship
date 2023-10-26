using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;

namespace Reminders.DAL.Interfaces;

public interface IReminderRepository : IBaseRepository<Reminder, ReminderFilter>
{
    
}