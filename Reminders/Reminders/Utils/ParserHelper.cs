using System.Globalization;

namespace Reminders.Services
{
    public class ParserHelper
    {
        public static (string name, DateTime date) ParseAddCommand(string userInput)
        {
            var parts = userInput.Split('"')
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .ToArray();
            
            ValidatePartsLength(parts, "add \"Name\" dd/mm/yyyy");
            
            var date = ParseDate(parts[2].Trim(), "dd/mm/yyyy");
            
            if (date.Date < DateTime.Today || date.Date.Year > 2100)
                throw new ArgumentException("Date cannot be in the past or distant future. Please enter the correct date.");

            return (parts[1], date);
        }

        public static int ParseDeleteCommand(string userInput)
        {
            if (int.TryParse(userInput.Split(' ')[1], out int id))
                return id;
            else
                throw new ArgumentException("Error input. ID should be a number.");
        }

        public static (DateTime date, bool olderThanDate) ParseFilterCommand(string userInput)
        {
            var parts = userInput.Split(' ');

            ValidatePartsLength(parts, "filter <before/after> dd/mm/yyyy");
            
            var date = ParseDate(parts[2].Trim(), "dd/mm/yyyy");

            var olderThanDate = parts[1].ToLower() switch
            {
                "after" => true,
                "before" => false,
                _ => throw new ArgumentException("Incorrect format.")
            };

            return (date, olderThanDate);
        }

        public static DateTime ParseGetRemindersByDateCommand(string userInput)
        {
            var parts = userInput.Split(' ');
            
            if (!DateTime.TryParseExact(parts[1].Trim(), "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                throw new ArgumentException($"Incorrect date format. Please use like this: MM/yyyy");

            return date;
        }
        
        public static string[] ParseSearchCommand(string userInput)
        {
            var keywords = userInput.ToLower().Split(" ").Skip(1).ToArray();
            if (keywords.Length > 0)
                return keywords;
            else
                throw new ArgumentException("Error input.");
        }
        
        private static void ValidatePartsLength(string[] parts, string expectedFormat)
        {
            if (parts.Length < 3)
                throw new ArgumentException($"Incorrect format. Please use like this: {expectedFormat}");
        }

        private static DateTime ParseDate(string dateString, string expectedFormat)
        {
            if (!DateTime.TryParse(dateString, out DateTime date))
                throw new ArgumentException($"Incorrect date format. Please use like this: {expectedFormat}");

            return date;
        }
    }
}
