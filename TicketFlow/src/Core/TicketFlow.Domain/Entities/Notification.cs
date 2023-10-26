using TicketFlow.Domain.Enums.Notifications;

namespace TicketFlow.Domain.Entities;

public class Notification
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime Timestamp { get; set; }
    
    public User User { get; set; }
}