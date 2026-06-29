using Microsoft.AspNetCore.Mvc;
using TaskFlow.Data;
using TaskFlow.Helpers;
using TaskFlow.Interfaces;

namespace TaskFlow.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(ICurrentUserServices currentUser, AppDbContext db, INotificationService notificationService) : base(currentUser,db) { 

            _notificationService = notificationService;
        }

        // GET: /Notification
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var notifications = await _notificationService.GetNotificationsAsync(EmployeeId);

            return View(notifications);
        }

        // GET: /Notification/GetUnreadCount
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var unreadCount = await _notificationService.GetUnreadCountAsync(EmployeeId);

            return Json(new
            {
                count = unreadCount
            });
        }

        // GET: /Notification/GetRecentNotifications
        [HttpGet]
        public async Task<IActionResult> GetRecentNotifications()
        {
            var notifications = await _notificationService.GetRecentNotificationsAsync(EmployeeId);

            return PartialView("_NotificationDropdown", notifications);
        }

        [HttpGet]
        public async Task<IActionResult> Open(long id)
        {
            var notification = await _notificationService.OpenNotificationAsync(id);

            if (notification == null)
            {
                TempData["Error"] = "Notification not found.";
                return RedirectToAction(nameof(Index));
            }

            switch (notification.ReferenceType)
            {
                case ReferenceType.Task:
                    return RedirectToAction(
                        "Details",
                        "EmployeeTask",
                        new { id = notification.ReferenceId });

                case ReferenceType.Project:
                    return RedirectToAction(
                        "Details",
                        "Project",
                        new { id = notification.ReferenceId });

                case ReferenceType.Employee:
                    return RedirectToAction(
                        "Details",
                        "Employee",
                        new { id = notification.ReferenceId });

                case ReferenceType.Department:
                    return RedirectToAction(
                        "Details",
                        "Department",
                        new { id = notification.ReferenceId });

                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Notification/MarkAsRead
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var success = await _notificationService.MarkAsReadAsync(id);

            return Json(new
            {
                success
            });
        }

        // POST: /Notification/MarkAllAsRead
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var success = await _notificationService.MarkAllAsReadAsync(EmployeeId);

            return Json(new
            {
                success
            });
        }
    }
}
