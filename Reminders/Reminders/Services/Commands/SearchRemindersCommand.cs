using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Utils;

namespace Reminders.Services.Commands
{
    public class SearchRemindersCommand : ICommand<GetOperationResult>
    {
        private readonly string _userInput;
        private readonly ReminderService _reminderService;
        private readonly RemindersFilter _remindersFilter;

        public SearchRemindersCommand(string userInput, ReminderService reminderService, RemindersFilter remindersFilter)
        {
            _userInput = userInput;
            _reminderService = reminderService;
            _remindersFilter = remindersFilter;
        }

        public async Task<GetOperationResult> ExecuteAsync()
        {
            _remindersFilter.Name = ParserHelper.ParseSearchCommand(_userInput);
            var searchResult = await _reminderService.GetRemindersByFilterAsync(_remindersFilter);
            ConsoleHelper.PrintColoredMessage(searchResult.Message, ConsoleColor.DarkGreen);
            return searchResult;
        }
    }   
}