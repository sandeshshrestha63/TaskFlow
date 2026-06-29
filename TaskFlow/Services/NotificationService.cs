using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs.Notification;
using TaskFlow.Interfaces;
using TaskFlow.Models;

namespace TaskFlow.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IDateTimeService _dateTimeService;

        public NotificationService(AppDbContext context, IDateTimeService dateTimeService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        public async Task<int> CreateNotificationAsync(
           NotificationRequest request,
           IEnumerable<int> employeeIds)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.CompanyId <= 0)
                throw new ArgumentException("A valid CompanyId is required.", nameof(request.CompanyId));

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Notification title is required.", nameof(request.Title));

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException("Notification message is required.", nameof(request.Message));

            if (employeeIds == null)
                return 0;

            var recipientIds = employeeIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (!recipientIds.Any())
                return 0;

            var createdDate = _dateTimeService.UtcNow;

            var notifications = recipientIds
                .Select(employeeId => CreateNotification(request, employeeId, createdDate))
                .ToList();

            await _context.Notifications.AddRangeAsync(notifications);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("A valid EmployeeId is required.", nameof(employeeId));

            return await _context.Notifications
                .Where(n => n.EmployeeId == employeeId
                         && !n.IsDeleted
                         && !n.IsRead)
                .CountAsync();
        }
        public async Task<NotificationResponse?> OpenNotificationAsync(long notificationId)
        {
            if (notificationId <= 0)
                throw new ArgumentException("A valid NotificationId is required.", nameof(notificationId));

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x =>
                    x.NotificationId == notificationId &&
                    !x.IsDeleted);

            if (notification == null)
                return null;

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadDate = _dateTimeService.UtcNow;

                await _context.SaveChangesAsync();
            }

            return new NotificationResponse
            {
                NotificationId = notification.NotificationId,
                Title = notification.Title,
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                ReferenceType = notification.ReferenceType,
                ReferenceId = notification.ReferenceId,
                Priority = notification.Priority,
                IsRead = notification.IsRead,
                CreatedDate = notification.CreatedDate
            };
        }
        public async Task<NotificationResponse?> GetNotificationByIdAsync(long notificationId)
        {
            if (notificationId <= 0)
                throw new ArgumentException("A valid NotificationId is required.", nameof(notificationId));

            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.NotificationId == notificationId && !n.IsDeleted)
                .Select(n => new NotificationResponse
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    ReferenceType = n.ReferenceType,
                    ReferenceId = n.ReferenceId,
                    Priority = n.Priority,
                    IsRead = n.IsRead,
                    CreatedDate = n.CreatedDate
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<NotificationResponse>> GetRecentNotificationsAsync(int employeeId, int take = 10)
        {
            if (employeeId <= 0)
                throw new ArgumentException("A valid EmployeeId is required.", nameof(employeeId));

            if (take <= 0)
                take = 10;

            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.EmployeeId == employeeId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedDate)
                .Take(take)
                .Select(n => new NotificationResponse
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    ReferenceType = n.ReferenceType,
                    ReferenceId = n.ReferenceId,
                    Priority = n.Priority,
                    IsRead = n.IsRead,
                    CreatedDate = n.CreatedDate
                })
                .ToListAsync();
        }
        public async Task<List<NotificationResponse>> GetNotificationsAsync(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("A valid EmployeeId is required.", nameof(employeeId));

            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.EmployeeId == employeeId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NotificationResponse
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    ReferenceType = n.ReferenceType,
                    ReferenceId = n.ReferenceId,
                    Priority = n.Priority,
                    IsRead = n.IsRead,
                    CreatedDate = n.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(long notificationId)
        {
            if (notificationId <= 0)
                throw new ArgumentException("A valid NotificationId is required.", nameof(notificationId));

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId && !n.IsDeleted);

            if (notification == null)
                return false;

            if (notification.IsRead)
                return true;

            notification.IsRead = true;
            notification.ReadDate = _dateTimeService.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("A valid EmployeeId is required.", nameof(employeeId));

            var notifications = await _context.Notifications.Where(n => n.EmployeeId == employeeId && !n.IsDeleted && !n.IsRead).ToListAsync();

            if (!notifications.Any())
                return false;

            var readDate = _dateTimeService.UtcNow;

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadDate = readDate;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        private Notification CreateNotification(
            NotificationRequest request,
            int employeeId,
            DateTime createdDate)
        {
            return new Notification
            {
                CompanyId = request.CompanyId,
                EmployeeId = employeeId,

                Title = request.Title,
                Message = request.Message,

                NotificationType = request.NotificationType,
                ReferenceType = request.ReferenceType,
                ReferenceId = request.ReferenceId,

                Priority = request.Priority,

                IsRead = false,

                CreatedBy = request.CreatedBy,
                CreatedDate = createdDate,

                IsDeleted = false
            };
        }
    }
}