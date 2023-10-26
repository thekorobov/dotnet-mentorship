using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Utils;

namespace Reminders.Services.Commands
{
    public class AddReminderCommand : ICommand<BaseOperationResult>
    {
        private readonly string _userInput;
        private readonly ReminderService _reminderService;

        public AddReminderCommand(string userInput, ReminderService reminderService)
        {
            _userInput = userInput;
            _reminderService = reminderService;
        }
        
        public async Task<BaseOperationResult> ExecuteAsync()
        {
            var (name, date) = ParserHelper.ParseAddCommand(_userInput);
            var addResult = await _reminderService.AddReminderAsync(name, date);
            ConsoleHelper.PrintColoredMessage(addResult.Message, ConsoleColor.DarkGreen);
            return addResult;
        }
    }
}