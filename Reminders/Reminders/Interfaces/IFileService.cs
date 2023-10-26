using Reminders.Models;

namespace Reminders.Interfaces
{
    public interface IFileService
    {
        Task SaveReminderAsync(Reminder reminder);
        Task<List<Reminder>?> LoadRemindersAsync();
        Task DeleteReminderAsync(int id, DateTime date);
        Task SaveRemindersAsync(DateTime date, List<Reminder> reminders);
        Task<List<Reminder>?> LoadRemindersByDateAsync(DateTime? date);
    }   
}