using Reminders.Interfaces;
using Reminders.Models;

namespace Reminders.Services
{
    public class ThreadSafeFileService : IFileService
    {
        private readonly FileService  _fileService;
        private readonly SemaphoreSlim _semaphoreSlim;

        public ThreadSafeFileService(FileService fileService, SemaphoreSlim semaphoreSlim)
        {
            _fileService = fileService;
            _semaphoreSlim = semaphoreSlim;
        }

        public async Task SaveReminderAsync(Reminder reminder)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await _fileService.SaveReminderAsync(reminder);
            }
            finally
            {
                _semaphoreSlim.Release();   
            }
        }

        public async Task<List<Reminder>?> LoadRemindersAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                return await _fileService.LoadRemindersAsync();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task DeleteReminderAsync(int id, DateTime date)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await _fileService.DeleteReminderAsync(id, date);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task SaveRemindersAsync(DateTime date, List<Reminder> reminders)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                await _fileService.SaveRemindersAsync(date, reminders);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<List<Reminder>?> LoadRemindersByDateAsync(DateTime? date)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                return await _fileService.LoadRemindersByDateAsync(date);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }   
}