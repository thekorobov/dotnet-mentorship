using Reminders.Models;

namespace Reminders.Services
{
    public class ReminderService
    {
        private List<Reminder> _reminders = new List<Reminder>();
        private static FileService _fileService = new FileService();

        private ThreadSafeFileService _threadSafeFileService =
            new ThreadSafeFileService(_fileService, new SemaphoreSlim(2));

        private static int _nextId = 0;

        public async Task LoadFromFileAsync()
        {
            _reminders = await _threadSafeFileService.LoadRemindersAsync();
            if (_reminders!.Any())
            {
                _nextId = _reminders!.Max(r => r.Id);
            }
        }

        private async Task SaveToFileAsync(Reminder reminder)
        {
            await _threadSafeFileService.SaveReminderAsync(reminder);
        }

        public async Task<BaseOperationResult> AddReminderAsync(string name, DateTime date)
        {
            var reminder = new Reminder
            {
                Id = ++_nextId,
                Name = name,
                Date = date
            };

            _reminders.Add(reminder);

            await SaveToFileAsync(reminder);

            return new BaseOperationResult
            {
                Success = true,
                Message = $"Added reminder with ID: {reminder.Id}\n"
            };
        }

        public async Task<BaseOperationResult> DeleteReminderByIdAsync(int id)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == id);

            if (reminder != null)
            {
                _reminders.Remove(reminder);

                await _threadSafeFileService.DeleteReminderAsync(id, reminder.Date);

                return new BaseOperationResult
                {
                    Success = true,
                    Message = $"Reminder with ID: {id} has been removed.\n"
                };
            }
            else
            {
                return new BaseOperationResult
                {
                    Success = false,
                    Message = $"Reminder with ID: {id} not found.\n"
                };
            }
        }

        public async Task<GetOperationResult> GetRemindersByFilterAsync(RemindersFilter filter)
        {
            var filteredReminders = new List<Reminder>();

            if (filter.Name != null && filter.Name.Length > 0)
            {
                filteredReminders = _reminders
                    .Where(r => filter.Name.Any(name => r.Name!.ToLower().Contains(name))).ToList();
            }
            else if (filter.ToNotify)
            {
                filteredReminders = _reminders
                    .Where(r => r.Date <= filter.NotifyDate)
                    .OrderBy(r => r.Date)
                    .ToList();
            }

            if (filter.Date.HasValue)
            {
                filteredReminders = await _threadSafeFileService.LoadRemindersByDateAsync(filter.Date);
            }
            else if (filter.MaxDate.HasValue)
            {
                filteredReminders = _reminders.Where(r => r.Date > filter.MaxDate).OrderBy(r => r.Date).ToList();
            }
            else if (filter.MinDate.HasValue)
            {
                filteredReminders = _reminders.Where(r => r.Date < filter.MinDate).OrderBy(r => r.Date).ToList();
            }
            
            if (filteredReminders.Count == 0 && !filter.Date.HasValue && !filter.MaxDate.HasValue && !filter.MinDate.HasValue && (filter.Name?.Length == 0 || filter.Name == null))
            {
                filteredReminders = _reminders.OrderBy(r => r.Date).ToList();
            }

            if (!filteredReminders.Any())
            {
                return new GetOperationResult
                {
                    Success = false,
                    Message = "No reminders found.\n"
                };
            }
            else
            {
                return new GetOperationResult
                {
                    Success = true,
                    Message = filter.ToMessage(filteredReminders),
                    Reminders = filteredReminders
                };
            }
        }
    }
}