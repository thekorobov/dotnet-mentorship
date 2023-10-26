using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Utils;

namespace Reminders.Services.Commands
{
    public class AllRemindersByDateCommand : ICommand<GetOperationResult>
    {
        private readonly string _userInput;
        private readonly ReminderService _reminderService;
        private readonly RemindersFilter _remindersFilter;

        public AllRemindersByDateCommand(string userInput, ReminderService reminderService, RemindersFilter remindersFilter)
        {
            _userInput = userInput;
            _reminderService = reminderService;
            _remindersFilter = remindersFilter;
        }
        
        public async Task<GetOperationResult> ExecuteAsync()
        {
            _remindersFilter.Date = ParserHelper.ParseGetRemindersByDateCommand(_userInput);
            var allRemindersByDateResult = await _reminderService.GetRemindersByFilterAsync(_remindersFilter);
            ConsoleHelper.PrintColoredMessage(allRemindersByDateResult.Message, ConsoleColor.DarkGreen);
            return allRemindersByDateResult;
        }
    }   
}