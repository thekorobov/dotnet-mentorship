using Reminders.Models;
using System.Text.Json;
using Reminders.Interfaces;

namespace Reminders.Services
{
    public class FileService : IFileService
    {
        private const string PathPrefix = "reminders_";
        private const string Extension = ".json";

        public async Task SaveReminderAsync(Reminder reminder)
        {
            var path = GetPath(reminder.Date);
            List<Reminder>? reminders;

            if (File.Exists(path))
            {
                var jsonData = await File.ReadAllTextAsync(path);
                reminders = JsonSerializer.Deserialize<List<Reminder>>(jsonData);
            }
            else
            {
                reminders = new List<Reminder>();
            }

            reminders?.Add(reminder);
            var newJsonData = JsonSerializer.Serialize(reminders);
            await File.WriteAllTextAsync(path, newJsonData);
        }

        public async Task<List<Reminder>?> LoadRemindersAsync()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), $"{PathPrefix}*{Extension}");
            var allReminders = new List<Reminder>();

            foreach (var file in files)
            {
                var jsonData = await File.ReadAllTextAsync(file);
                var reminders = JsonSerializer.Deserialize<List<Reminder>>(jsonData);
                if (reminders != null) allReminders.AddRange(reminders);
            }

            return allReminders;
        }

        public async Task DeleteReminderAsync(int id, DateTime date)
        {
            var reminders = await LoadRemindersByDateAsync(date);
            if (reminders != null)
            {
                reminders.RemoveAll(r => r.Id == id);
                await SaveRemindersAsync(date, reminders);
            }
        }

        public async Task SaveRemindersAsync(DateTime date, List<Reminder> reminders)
        {
            var path = GetPath(date);
            var jsonData = JsonSerializer.Serialize(reminders);
            await File.WriteAllTextAsync(path, jsonData);
        }

        public async Task<List<Reminder>?> LoadRemindersByDateAsync(DateTime? date)
        {
            var path = GetPath(date);

            if (!File.Exists(path)) return new List<Reminder>();

            var jsonData = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<Reminder>>(jsonData);
        }

        private string GetPath(DateTime? date)
        {
            return $"{PathPrefix}{date:yyyy_MM}{Extension}";
        }
    }
}