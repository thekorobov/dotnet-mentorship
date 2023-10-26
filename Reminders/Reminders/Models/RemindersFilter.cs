namespace Reminders.Models
{
    public class RemindersFilter
    {
        public DateTime? Date { get; set; }
        public DateTime? MaxDate { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? NotifyDate { get; set; }
        public string[]? Name { get; set; }
        public bool ToNotify { get; set; }
        
        public string ToMessage(List<Reminder> filteredReminders)
        {
            var remindersCount = filteredReminders.Count;
            
            if (ToNotify && NotifyDate != null)
            {
                return $"Current date = {(NotifyDate?.ToString("dd/MM/yyyy") ?? "")}\nYou have some task(s) - {remindersCount}:\n";
            }
            else if (remindersCount > 0)
            {
                if (Date == null && !MaxDate.HasValue && !MinDate.HasValue && Name == null)
                {
                    return $"Reminders retrieved successfully. Count - {remindersCount}.";
                }
                else if (Date.HasValue)
                {
                    return $"Reminders by {Date?.ToString("MM/yyyy")} retrieved successfully. Count - {remindersCount}.";
                }
                else if (MaxDate.HasValue)
                {
                    return $"Reminders after {MaxDate?.ToString("dd/MM/yyyy")} retrieved successfully. Count - {remindersCount}.";
                }
                else if (MinDate.HasValue)
                {
                    return $"Reminders before {MinDate?.ToString("dd/MM/yyyy")} retrieved successfully. Count - {remindersCount}.";
                }
                else if (Name != null && Name.Length > 0)
                {
                    return $"{remindersCount} reminder(s) found:";
                }
            }

            return string.Empty;
        }
    }
}