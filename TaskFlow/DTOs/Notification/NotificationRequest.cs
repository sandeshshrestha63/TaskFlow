using TaskFlow.Helpers;

namespace TaskFlow.DTOs.Notification
{
    public class NotificationRequest
    {
        public int CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
        public int? CreatedBy { get; set; }
    }
}
