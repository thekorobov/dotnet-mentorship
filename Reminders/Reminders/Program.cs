using Reminders.Interfaces;
using Reminders.Models;
using Reminders.Services;
using Reminders.Services.Commands;
using Reminders.Utils;

namespace Reminders
{
    public class Program
    {
        private static ReminderService _reminderService = new ReminderService();
        
        static async Task Main(string[] args)
        {
            await _reminderService.LoadFromFileAsync();

            DisplayOptions();

            var cancellationTokenSource = new CancellationTokenSource();

            var reminderCheckerService = new RemindersBackgroundService(_reminderService, new DateTime(2023, 10, 1));
            reminderCheckerService.ReminderNotification += HandleReminderNotification;
            var reminderCheckerTask = reminderCheckerService.StartAsync(cancellationTokenSource.Token);
            
            var remindersFilter = new RemindersFilter();
            
            try
            {
                while (true)
                {
                    var userInput = Console.ReadLine();
                    if (userInput == "*")
                    {
                        cancellationTokenSource.Cancel();
                        break;
                    }
                    await HandleCommandAsync(userInput, remindersFilter);
                    remindersFilter = new RemindersFilter(); 
                }
            }
            finally
            {
                cancellationTokenSource.Cancel(); 
                await reminderCheckerTask; 
            }
        }
        
        private static void HandleReminderNotification(string notificationMessage)
        {
            Console.WriteLine(notificationMessage);
        }
        
        private static async Task HandleCommandAsync(string userInput, RemindersFilter remindersFilter)
        {
            try
            {
                var command = userInput.Split().FirstOrDefault();
                ICommand<BaseOperationResult> baseCommandToExecute;
                ICommand<GetOperationResult> getResultCommandToExecute;
                
                switch (command)
                {
                    case "add":
                        baseCommandToExecute = new AddReminderCommand(userInput, _reminderService);
                        await baseCommandToExecute.ExecuteAsync();
                        break;

                    case "delete":
                        baseCommandToExecute = new DeleteReminderCommand(userInput, _reminderService);
                        await baseCommandToExecute.ExecuteAsync();
                        break;

                    case "get-all":
                        getResultCommandToExecute = new AllRemindersCommand(_reminderService, remindersFilter);
                        var allRemindersResult = await getResultCommandToExecute.ExecuteAsync();

                        if (allRemindersResult.Success)
                            ConsoleHelper.PrintReminders(allRemindersResult.Reminders);
                        
                        break;
                    
                    case "get-by-date":
                        getResultCommandToExecute = new AllRemindersByDateCommand(userInput, _reminderService, remindersFilter);
                        var allRemindersByDateResult = await getResultCommandToExecute.ExecuteAsync();

                        if (allRemindersByDateResult.Success)
                            ConsoleHelper.PrintReminders(allRemindersByDateResult.Reminders);
                        
                        break;
                    
                    case "filter":
                        getResultCommandToExecute = new FilterRemindersCommand(userInput, _reminderService, remindersFilter);
                        var filterResult = await getResultCommandToExecute.ExecuteAsync();
                        
                        if (filterResult.Success)
                            ConsoleHelper.PrintReminders(filterResult.Reminders);
                        
                        break;
                    
                    case "search":
                        getResultCommandToExecute = new SearchRemindersCommand(userInput, _reminderService, remindersFilter);
                        var searchResult = await getResultCommandToExecute.ExecuteAsync();
                        
                        if (searchResult.Success)
                            ConsoleHelper.PrintReminders(searchResult.Reminders);
                        
                        break;
                    
                    case "*":
                        Environment.Exit(0);
                        break;

                    default:
                        ConsoleHelper.PrintColoredMessage("Error input. Try again!\n", ConsoleColor.DarkYellow);
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintColoredMessage($"{ex.Message}\n", ConsoleColor.DarkYellow);
            }
        }
        
        
        private static void DisplayOptions()
        {
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("It is Reminders application!");
            Console.WriteLine("Available commands:\n" +
                              "add - add reminder, args: [add \"Buy milk\" dd/MM/yyyy]\n" +
                              "delete - delete reminder by ID, args: [delete ID]\n" +
                              "filter - get reminders before/after a given date, args: [filter before/after dd/MM/yyyy]\n" +
                              "search - get reminders by name, args: [search name]\n" +
                              "get-all - get reminders info\n" +
                              "get-by-date - get reminders by date, args: [get-by-date MM/yyyy]\n\n" +
                              "To exit the program, enter *");
            Console.WriteLine(new string('-', 60) + "\n");
        }
    }
}
