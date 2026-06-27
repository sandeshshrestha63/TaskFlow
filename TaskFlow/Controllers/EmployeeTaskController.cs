using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels;
using TaskFlow.ViewModels.EmployeeTask;
using TaskFlow.Exceptions;
using TaskFlow.Services;
using TaskFlow.ViewModels.Requests.Attachments;

namespace TaskFlow.Controllers
{
    public class EmployeeTaskController : BaseController
    {
        private ITaskActivityService _activityService;
        private readonly ITaskAttachmentService _taskAttachmentService;
        public EmployeeTaskController(
     ICurrentUserServices currentUser,
     ITaskActivityService activityService,
     ITaskAttachmentService taskAttachmentService,
     AppDbContext db)
     : base(currentUser, db)
        {
            _activityService = activityService;
            _taskAttachmentService = taskAttachmentService;
        }
        public async Task<IActionResult> Index(string? searchText, int? employeeId, int? statusId, int? priorityId)
        {
            var companyId = CompanyId;

            var query = _db.EmployeeTasks.Include(x => x.AssignedToEmployee)
                            .Include(x => x.EmployeeTaskStatus)
                            .Include(x => x.EmployeeTaskPriority)
                            .Where(x => x.CompanyId == companyId && !x.IsDeleted)
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x => x.Title.Contains(searchText));
            }

            if (employeeId.HasValue)
            {
                query = query.Where(x => x.AssignedToEmployeeId == employeeId);
            }

            if (statusId.HasValue)
            {
                query = query.Where(x => x.EmployeeTaskStatusId == statusId);
            }

            if (priorityId.HasValue)
            {
                query = query.Where(x => x.EmployeeTaskPriorityId == priorityId);
            }

            var tasks = await query.OrderByDescending(x => x.CreatedDate).Select(x => new EmployeeTaskListVM
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedToEmployee.FirstName + " " + x.AssignedToEmployee.LastName,
                Priority = x.EmployeeTaskPriority.Name,
                Status = x.EmployeeTaskStatus.Name,
                DueDate = x.DueDate,
                CreatedDate = x.CreatedDate
            }).ToListAsync();

            ViewBag.Employees = await _db.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.FirstName).ToListAsync();

            ViewBag.Statuses = await _db.EmployeeTaskStatus.Where(x => x.CompanyId == companyId && x.IsActive).OrderBy(x => x.DisplayOrder).ToListAsync();

            ViewBag.Priorities = await _db.TaskPriorities.Where(x => x.CompanyId == companyId && x.IsActive).OrderBy(x => x.DisplayOrder).ToListAsync();

            return View(new TaskIndexVM
            {
                SearchText = searchText,
                EmployeeId = employeeId,
                StatusId = statusId,
                PriorityId = priorityId,
                Tasks = tasks
            });
        }
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View(new EmployeeTaskDataVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeTaskDataVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }

            var defaultStatus = await _db.EmployeeTaskStatus.Where(x => x.CompanyId == CompanyId && x.IsDefault && x.IsActive).FirstOrDefaultAsync();

            if (defaultStatus == null)
            {
                TempData["Error"] = "No default task status has been configured.";
                await LoadDropdowns();
                return View(vm);
            }
            var employeeTask = new EmployeeTask
            {
                Title = vm.Title,
                Description = vm.Description,

                CompanyId = CompanyId,

                CreatedByEmployeeId = EmployeeId,

                AssignedToEmployeeId = vm.AssignedToEmployeeId,

                EmployeeTaskPriorityId = vm.EmployeeTaskPriorityId,

                EmployeeTaskStatusId = defaultStatus.Id,

                DueDate = vm.DueDate,

                EstimatedHours = vm.EstimatedHours,

                CreatedDate = DateTime.UtcNow,

                IsDeleted = false
            };

            _db.EmployeeTasks.Add(employeeTask);

            await _db.SaveChangesAsync();
            await _activityService.AddActivityAsync(employeeTask.Id, EmployeeId, TaskActivityTypes.Created, $"Task '{employeeTask.Title}' was created.");

            TempData["Success"] = "Task created successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            var companyId = CompanyId;

            ViewBag.Employees = await _db.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.FirstName)
                                            .ToListAsync();
            ViewBag.Priorities = await _db.TaskPriorities.Where(x => x.CompanyId == companyId && x.IsActive).OrderBy(x => x.DisplayOrder)
                                             .ToListAsync();
        }
        public async Task<IActionResult> Edit(long id)
        {
            var companyId = CompanyId;

            var task = await _db.EmployeeTasks
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    !x.IsDeleted);

            if (task == null)
                return NotFound();

            await LoadDropdowns();

            var vm = new EmployeeTaskDataVM
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedToEmployeeId = task.AssignedToEmployeeId ?? 0,
                EmployeeTaskPriorityId = task.EmployeeTaskPriorityId,
                DueDate = task.DueDate,
                EstimatedHours = task.EstimatedHours
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeTaskDataVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }
            var companyId = CompanyId;

            var task = await _db.EmployeeTasks.FirstOrDefaultAsync(x => x.Id == vm.Id && x.CompanyId == companyId && !x.IsDeleted);

            if (task == null)
                return NotFound();

            var oldAssignedEmployeeId = task.AssignedToEmployeeId;
            var oldPriorityId = task.EmployeeTaskPriorityId;
            var oldDueDate = task.DueDate;

            task.Title = vm.Title;
            task.Description = vm.Description;
            task.AssignedToEmployeeId = vm.AssignedToEmployeeId;
            task.EmployeeTaskPriorityId = vm.EmployeeTaskPriorityId;
            task.DueDate = vm.DueDate;
            task.EstimatedHours = vm.EstimatedHours;
            task.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _activityService.AddActivityAsync(task.Id, EmployeeId, TaskActivityTypes.Updated, "Task details updated.");
            if (oldAssignedEmployeeId != vm.AssignedToEmployeeId)
            {
                await _activityService.AddActivityAsync(task.Id, EmployeeId, TaskActivityTypes.Assigned, "Task assignment was changed.");
            }
            if (oldPriorityId != vm.EmployeeTaskPriorityId)
            {
                var oldPriority = await _db.TaskPriorities.Where(x => x.Id == oldPriorityId).Select(x => x.Name).FirstOrDefaultAsync();

                var newPriority = await _db.TaskPriorities.Where(x => x.Id == vm.EmployeeTaskPriorityId).Select(x => x.Name).FirstOrDefaultAsync();

                await _activityService.AddActivityAsync(task.Id, EmployeeId, TaskActivityTypes.PriorityChanged, $"Priority changed from '{oldPriority}' to '{newPriority}'.");
            }

            SuccessMessage("Task updated successfully.");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(long id)
        {
            var companyId = CompanyId;

            var task = await _db.EmployeeTasks
                .Include(x => x.AssignedToEmployee)
                .Include(x => x.CreatedByEmployee)
                .Include(x => x.EmployeeTaskStatus)
                .Include(x => x.EmployeeTaskPriority)
                .Include(x => x.Comments)
                    .ThenInclude(c => c.CreatedByEmployee)
                .Where(x =>
                    x.CompanyId == companyId &&
                    !x.IsDeleted &&
                    x.Id == id)
                .Select(x => new TaskDetailsVM
                {
                    Id = x.Id,

                    Title = x.Title,

                    Description = x.Description,

                    AssignedTo =
                        x.AssignedToEmployee.FirstName + " " +
                        x.AssignedToEmployee.LastName,

                    CreatedBy =
                        x.CreatedByEmployee.FirstName + " " +
                        x.CreatedByEmployee.LastName,

                    Priority = x.EmployeeTaskPriority.Name,
                    PriorityColor = x.EmployeeTaskPriority.ColorCode,

                    Status = x.EmployeeTaskStatus.Name,
                    StatusColor = x.EmployeeTaskStatus.ColorCode,

                    DueDate = x.DueDate,

                    EstimatedHours = x.EstimatedHours,

                    ActualHours = x.ActualHours,

                    CreatedDate = x.CreatedDate,

                    CompletedDate = x.CompletedDate,
                    Comments = x.Comments.OrderByDescending(c => c.CreatedDate).Select(c => new TaskCommentListVM
                    {
                        EmployeeName = c.CreatedByEmployee.FirstName + " " + c.CreatedByEmployee.LastName,
                        Comment = c.Comment,
                        CreatedDate = c.CreatedDate
                    }).ToList(),
                    Activities = x.Activities.OrderByDescending(a => a.CreatedDate).Select(a => new TaskActivityVM
                    {
                        EmployeeName =
                            a.Employee.FirstName + " " +
                            a.Employee.LastName,

                        ActivityType = a.ActivityType,

                        Description = a.Description,

                        CreatedDate = a.CreatedDate
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if (task == null)
                return NotFound();

            task.Attachments = await _taskAttachmentService.GetTaskAttachmentsAsync(id, CompanyId);

            ViewBag.Statuses = await _db.EmployeeTaskStatus.Where(x => x.CompanyId == CompanyId && x.IsActive)
                                    .OrderBy(x => x.DisplayOrder).ToListAsync();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(TaskCommentVM vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = vm.TaskId });

            var task = await _db.EmployeeTasks
                .FirstOrDefaultAsync(x => 
                    x.Id == vm.TaskId &&
                    x.CompanyId == CompanyId);

            if (task == null)
                return NotFound();

            var comment = new TaskComment
            {
                EmployeeTaskId = vm.TaskId,
                CreatedByEmployeeId = EmployeeId,
                Comment = vm.Comment,
                CreatedDate = DateTime.UtcNow
            };

            _db.TaskComments.Add(comment);

            await _db.SaveChangesAsync();

            SuccessMessage("Comment added successfully.");

            return RedirectToAction(nameof(Details), new { id = vm.TaskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAttachment(UploadTaskAttachmentsRequest request)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = request.TaskId });

            try
            {
                request.CompanyId = CompanyId;
                request.EmployeeId = EmployeeId;

                var attachments = await _taskAttachmentService.UploadAsync(request);

                await _activityService.AddActivityAsync(
                    request.TaskId,
                    EmployeeId,
                    TaskActivityTypes.AttachmentUploaded,
                    $"Uploaded {attachments.Count} attachment(s).");

                SuccessMessage($"{attachments.Count} attachment(s) uploaded successfully.");
            }
            catch (TaskFlowException ex)
            {
                ErrorMessage(ex.Message);
            }
            catch (Exception)
            {
                ErrorMessage("An unexpected error occurred while uploading attachments.");
            }

            return RedirectToAction(nameof(Details), new { id = request.TaskId });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAttachment(long attachmentId)
        {
            try
            {
                var response = await _taskAttachmentService.DownloadAsync(
                    new DownloadAttachmentRequest
                    {
                        AttachmentId = attachmentId,
                        CompanyId = CompanyId
                    });

                return File(
                    response.FileBytes,
                    response.ContentType,
                    response.FileName);
            }
            catch (TaskFlowException ex)
            {
                ErrorMessage(ex.Message);
            }
            catch (Exception)
            {
                ErrorMessage("Unable to download the selected attachment.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttachment(DeleteAttachmentRequest request)
        {
            try
            {
                var attachment = await _taskAttachmentService.GetAttachmentAsync(
                    request.AttachmentId,
                    CompanyId);

                request.CompanyId = CompanyId;

                await _taskAttachmentService.DeleteAsync(request);

                await _activityService.AddActivityAsync(
                    attachment.EmployeeTaskId,
                    EmployeeId,
                    TaskActivityTypes.AttachmentDeleted,
                    $"Deleted attachment '{attachment.OriginalFileName}'.");

                SuccessMessage("Attachment deleted successfully.");

                return RedirectToAction(nameof(Details), new
                {
                    id = attachment.EmployeeTaskId
                });
            }
            catch (TaskFlowException ex)
            {
                ErrorMessage(ex.Message);
            }
            catch (Exception)
            {
                ErrorMessage("Unable to delete the attachment.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateTaskStatusVM vm)
        {
            var task = await _db.EmployeeTasks
                .FirstOrDefaultAsync(x =>
                    x.Id == vm.TaskId &&
                    x.CompanyId == CompanyId);

            if (task == null)
                return NotFound();

            var oldStatusId = task.EmployeeTaskStatusId;

            task.EmployeeTaskStatusId = vm.StatusId;
            task.UpdatedDate = DateTime.UtcNow;

            var completedStatus = await _db.EmployeeTaskStatus.FirstOrDefaultAsync(x => x.Id == vm.StatusId && x.CompanyId == CompanyId);

            if (completedStatus != null &&
                completedStatus.Name.ToLower() == "completed")
            {
                task.CompletedDate = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();

            if (oldStatusId != task.EmployeeTaskStatusId)
            {
                var oldStatus = await _db.EmployeeTaskStatus.Where(x => x.Id == oldStatusId).Select(x => x.Name).FirstOrDefaultAsync();

                var newStatus = await _db.EmployeeTaskStatus.Where(x => x.Id == vm.StatusId).Select(x => x.Name).FirstOrDefaultAsync();

                await _activityService.AddActivityAsync(task.Id, EmployeeId, TaskActivityTypes.StatusChanged, $"Status changed from '{oldStatus}' to '{newStatus}'.");
            }


            SuccessMessage("Task status updated successfully.");

            return RedirectToAction(nameof(Details), new { id = vm.TaskId });
        }
    }
}
