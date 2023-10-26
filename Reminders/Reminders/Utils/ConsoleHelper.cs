using Reminders.Models;

namespace Reminders.Utils
{
    public static class ConsoleHelper
    {
        public static void PrintColoredMessage(string? message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void PrintReminders(List<Reminder>? reminders)
        {
            foreach (var reminder in reminders!)
            {
                Console.WriteLine($"ID: {reminder.Id}, Name: {reminder.Name}, Date: {reminder.Date:dd/MM/yyyy}");
            }
            Console.WriteLine();
        }
    }
}

