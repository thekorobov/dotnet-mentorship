using System.Text;
using Reminders.Models;

namespace Reminders.Services
{
    public class RemindersBackgroundService
    {
        private readonly ReminderService _reminderService;
        private static DateTime _currentDate;
        
        public delegate void ReminderNotificationHandler(string notificationMessage);
        public event ReminderNotificationHandler ReminderNotification;

        public RemindersBackgroundService(ReminderService reminderService, DateTime date)
        {
            _reminderService = reminderService;
            _currentDate = date;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _currentDate = _currentDate.AddDays(15);
                    var remindersFilter = new RemindersFilter
                    {
                        NotifyDate = _currentDate,
                        ToNotify = true
                    };

                    var remindersToNotify = await _reminderService.GetRemindersByFilterAsync(remindersFilter);

                    var notificationMessage = new StringBuilder(remindersToNotify.Message);
                    foreach (var reminder in remindersToNotify.Reminders)
                    {
                        notificationMessage.AppendLine(
                            $"ID: {reminder.Id}. It's time to \"{reminder.Name}\", deadline - {reminder.Date:dd/MM/yyyy}.");
                    }

                    HandleReminderNotification(notificationMessage.ToString());

                    await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
        }
        
        private void HandleReminderNotification(string notificationMessage)
        {
            ReminderNotification?.Invoke(notificationMessage);
        }
    }
}