using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Utils;

namespace Reminders.Services.Commands
{
    public class AllRemindersCommand : ICommand<GetOperationResult>
    {
        private readonly ReminderService _reminderService;
        private readonly RemindersFilter _remindersFilter;

        public AllRemindersCommand(ReminderService reminderService, RemindersFilter remindersFilter)
        {
            _reminderService = reminderService;
            _remindersFilter = remindersFilter;
        }
        
        public async Task<GetOperationResult> ExecuteAsync()
        {
            var allRemindersResult = await _reminderService.GetRemindersByFilterAsync(_remindersFilter);
            ConsoleHelper.PrintColoredMessage(allRemindersResult.Message, ConsoleColor.DarkGreen);
            return allRemindersResult;
        }
    }    
}