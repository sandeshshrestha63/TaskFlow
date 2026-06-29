using TaskFlow.Helpers;

namespace TaskFlow.DTOs.Notification
{
    public class NotificationResponse
    {
        public long NotificationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public NotificationPriority Priority { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
