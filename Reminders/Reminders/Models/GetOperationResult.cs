namespace Reminders.Models
{
    public class GetOperationResult : BaseOperationResult
    {
        public List<Reminder>? Reminders { get; set; }
    }
}
