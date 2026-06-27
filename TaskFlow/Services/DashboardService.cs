using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels.Dashboard;

namespace TaskFlow.Services
{
    public class DashboardService : IDashboardService
    {
        AppDbContext _db;
        public DashboardService(AppDbContext context)
        {
            _db = context;
        }

        public async Task<EmployeeDashboardVM> GetEmployeeDashboardAsync(int companyId, int employeeId)
        {
            EmployeeDashboardVM vm = new();

            DateTime today = DateTime.Today;

            // Get Completed Status Id
            int? completedStatusId = await _db.EmployeeTaskStatus.AsNoTracking().Where(x => x.CompanyId == companyId && x.Name == "Completed").Select(x => (int?)x.Id).FirstOrDefaultAsync();

            // Get In Progress Status Id
            int? inProgressStatusId = await _db.EmployeeTaskStatus.AsNoTracking().Where(x => x.CompanyId == companyId && x.Name == "In Progress").Select(x => (int?)x.Id).FirstOrDefaultAsync();

            IQueryable<EmployeeTask> taskQuery = _db.EmployeeTasks.AsNoTracking().Where(x => x.CompanyId == companyId && x.AssignedToEmployeeId == employeeId && !x.IsDeleted);

            // Dashboard Statistics
            vm.MyTasks = await taskQuery.CountAsync();

            if (completedStatusId.HasValue)
            {
                vm.CompletedTasks = await taskQuery.CountAsync(x => x.EmployeeTaskStatusId == completedStatusId.Value);
            }

            if (inProgressStatusId.HasValue)
            {
                vm.InProgressTasks = await taskQuery.CountAsync(x => x.EmployeeTaskStatusId == inProgressStatusId.Value);
            }

            vm.DueToday = await taskQuery.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date == today);

            if (completedStatusId.HasValue)
            {
                vm.OverdueTasks = await taskQuery.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date < today && x.EmployeeTaskStatusId != completedStatusId.Value);
            }

            // My Tasks
            vm.MyTaskList = await taskQuery.OrderByDescending(x => x.DueDate.HasValue && x.DueDate.Value.Date < today && (!completedStatusId.HasValue || x.EmployeeTaskStatusId != completedStatusId.Value))
                .ThenBy(x => x.DueDate)
                .ThenByDescending(x => x.EmployeeTaskPriority.DisplayOrder)
                .Take(5)
                .Select(x => new EmployeeTaskSummaryVM
                {
                    Id = x.Id,
                    Title = x.Title,

                    Status = x.EmployeeTaskStatus.Name,
                    StatusColor = x.EmployeeTaskStatus.ColorCode,

                    Priority = x.EmployeeTaskPriority.Name,
                    PriorityColor = x.EmployeeTaskPriority.ColorCode,

                    DueDate = x.DueDate,

                    IsOverdue =
                        x.DueDate.HasValue &&
                        x.DueDate.Value.Date < today &&
                        (!completedStatusId.HasValue ||
                         x.EmployeeTaskStatusId != completedStatusId.Value),

                    AssignedBy = x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName
                })
                .ToListAsync();
            vm.FocusTask = vm.MyTaskList.FirstOrDefault();

            // Recent Activity

            vm.RecentActivities = await _db.TaskActivities.AsNoTracking().Where(x => x.EmployeeTask.CompanyId == companyId && x.EmployeeTask.AssignedToEmployeeId == employeeId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(10)
                .Select(x => new DashboardActivityVM
                {
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,

                    ActivityType = x.ActivityType,

                    Description = x.Description,

                    CreatedDate = x.CreatedDate,

                    TaskId = x.EmployeeTaskId,

                    TaskTitle = x.EmployeeTask.Title
                })
                .ToListAsync();
            vm.UpcomingTasks = await _db.EmployeeTasks.AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.AssignedToEmployeeId == employeeId &&
                    !x.IsDeleted &&
                    x.DueDate.HasValue &&
                    x.DueDate.Value.Date >= today)
                .OrderBy(x => x.DueDate)
                .ThenByDescending(x => x.EmployeeTaskPriority.DisplayOrder)
                .Take(7)
                .Select(x => new EmployeeTaskSummaryVM
                {
                    Id = x.Id,
                    Title = x.Title,

                    Status = x.EmployeeTaskStatus.Name,
                    StatusColor = x.EmployeeTaskStatus.ColorCode,

                    Priority = x.EmployeeTaskPriority.Name,
                    PriorityColor = x.EmployeeTaskPriority.ColorCode,

                    DueDate = x.DueDate,

                    AssignedBy = x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName,

                    IsOverdue = false
                })
                .ToListAsync();
            vm.TotalDueToday = await taskQuery.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date == today);

            if (completedStatusId.HasValue)
            {
                vm.CompletedToday = await taskQuery.CountAsync(x =>
                    x.CompletedDate.HasValue &&
                    x.CompletedDate.Value.Date == today &&
                    x.EmployeeTaskStatusId == completedStatusId.Value);
            }

            vm.CompletionPercentage = vm.TotalDueToday == 0
                ? 100
                : (int)Math.Round((double)vm.CompletedToday * 100 / vm.TotalDueToday);
            return vm;
        }

        public async Task<CompanyDashboardVM> GetCompanyDashboardAsync(int companyId)
        {
            return new CompanyDashboardVM();
        }

        public async Task<SuperAdminDashboardVM> GetSuperAdminDashboardAsync()
        {
            return new SuperAdminDashboardVM();
        }
    }
}
