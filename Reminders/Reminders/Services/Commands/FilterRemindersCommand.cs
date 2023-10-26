using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Utils;

namespace Reminders.Services.Commands
{
    public class FilterRemindersCommand : ICommand<GetOperationResult>
    {
        private readonly string _userInput;
        private readonly ReminderService _reminderService;
        private readonly RemindersFilter _remindersFilter;

        public FilterRemindersCommand(string userInput, ReminderService reminderService, RemindersFilter remindersFilter)
        {
            _userInput = userInput;
            _reminderService = reminderService;
            _remindersFilter = remindersFilter;
        }

        public async Task<GetOperationResult> ExecuteAsync()
        {
            var (filterDate, olderThanDate) = ParserHelper.ParseFilterCommand(_userInput);
            if (olderThanDate)
                _remindersFilter.MaxDate = filterDate;
            else
                _remindersFilter.MinDate = filterDate;
                        
            var filterResult = await _reminderService.GetRemindersByFilterAsync(_remindersFilter);
            ConsoleHelper.PrintColoredMessage(filterResult.Message, ConsoleColor.DarkGreen);

            return filterResult;
        }
    }   
}