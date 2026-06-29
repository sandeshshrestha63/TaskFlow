using TaskFlow.DTOs.Notification;

namespace TaskFlow.Interfaces
{
    public interface INotificationService
    {
        Task<int> CreateNotificationAsync(NotificationRequest request, IEnumerable<int> employeeIds);
        Task<int> GetUnreadCountAsync(int employeeId);
        Task<NotificationResponse?> OpenNotificationAsync(long notificationId);
        Task<List<NotificationResponse>> GetRecentNotificationsAsync(int employeeId, int take = 10);
        Task<List<NotificationResponse>> GetNotificationsAsync(int employeeId);
        Task<bool> MarkAsReadAsync(long notificationId);
        Task<bool> MarkAllAsReadAsync(int employeeId);
    }
}
