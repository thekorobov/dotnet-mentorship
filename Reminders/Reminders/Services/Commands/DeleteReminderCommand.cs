using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Utils;

namespace Reminders.Services.Commands
{
    public class DeleteReminderCommand : ICommand<BaseOperationResult>
    {
        private readonly string _userInput;
        private readonly ReminderService _reminderService;

        public DeleteReminderCommand(string userInput, ReminderService reminderService)
        {
            _userInput = userInput;
            _reminderService = reminderService;
        }


        public async Task<BaseOperationResult> ExecuteAsync()
        {
            var id = ParserHelper.ParseDeleteCommand(_userInput);
            var deleteResult = await _reminderService.DeleteReminderByIdAsync(id);
            ConsoleHelper.PrintColoredMessage(deleteResult.Message, ConsoleColor.Red);
            return deleteResult;
        }
    }
}

