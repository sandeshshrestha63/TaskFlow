using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels.Dashboard;
using TaskFlow.ViewModels.EmployeeTask;

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
            var today = DateTime.Today;

            var vm = new CompanyDashboardVM();

            //==========================================================
            // Statistics
            //==========================================================

            vm.TotalEmployees = await _db.Employees.AsNoTracking().CountAsync(x => x.CompanyId == companyId);

            var taskQuery = _db.EmployeeTasks.AsNoTracking().Where(x => x.CompanyId == companyId && !x.IsDeleted);

            vm.ActiveTasks = await taskQuery.CountAsync(x => x.CompletedDate == null);

            vm.CompletedToday = await taskQuery.CountAsync(x => x.CompletedDate.HasValue && x.CompletedDate.Value.Date == today);

            vm.OverdueTasks = await taskQuery.CountAsync(x => x.CompletedDate == null && x.DueDate.HasValue && x.DueDate.Value.Date < today);

            // Projects module not implemented yet
            vm.ActiveProjects = 0;

            var employeeTaskSummary = await _db.Employees.Include(a => a.AssignedTasks).AsNoTracking().Where(e => e.CompanyId == companyId)
            .Select(e => new
            {
                EmployeeId = e.Id,

                EmployeeName = e.FirstName + " " + e.LastName,

                ActiveTasks = e.AssignedTasks.Count(t =>
                    !t.IsDeleted &&
                    t.CompletedDate == null),

                OverdueTasks = e.AssignedTasks.Count(t =>
                    !t.IsDeleted &&
                    t.CompletedDate == null &&
                    t.DueDate < today),

                DueTodayTasks = e.AssignedTasks.Count(t =>
                    !t.IsDeleted &&
                    t.CompletedDate == null &&
                    t.DueDate == today)
            })
            .ToListAsync();

            //==========================================================
            // Team Insights
            //==========================================================

            var completedTasks = await taskQuery
                .Where(x => x.CompletedDate.HasValue)
                .ToListAsync();

            var totalTaskCount = await taskQuery.CountAsync();

            vm.TeamInsights = new TeamInsightsVM
            {
                // Employee Statistics
                TotalEmployees = employeeTaskSummary.Count,

                EmployeesWithoutTasks = employeeTaskSummary.Count(x => x.ActiveTasks == 0),

                BalancedEmployees = employeeTaskSummary.Count(x => x.ActiveTasks > 0 && x.ActiveTasks <= 5),

                BusyEmployees = employeeTaskSummary.Count(x => x.ActiveTasks > 5 && x.ActiveTasks <= 10),

                OverloadedEmployees = employeeTaskSummary.Count(x => x.ActiveTasks > 10),

                AverageTasksPerEmployee = employeeTaskSummary.Any()
                    ? Math.Round((decimal)employeeTaskSummary.Average(x => x.ActiveTasks), 1)
                    : 0,

                // Task Statistics
                TotalActiveTasks = vm.ActiveTasks,

                TasksCompleted = completedTasks.Count,

                CompletionRate = totalTaskCount == 0
                    ? 0
                    : Math.Round((decimal)completedTasks.Count * 100 / totalTaskCount, 1),

                AverageCompletionDays = completedTasks.Any()
                    ? Math.Round(
                        (decimal)completedTasks.Average(x =>
                            (x.CompletedDate!.Value - x.CreatedDate).TotalDays),
                        1)
                    : 0
            };

            vm.EmployeesNeedingAttention = await _db.Employees.Include(a => a.AssignedTasks).AsNoTracking().Where(e => e.CompanyId == companyId).Select(e => new EmployeeAttentionVM
            {
                EmployeeId = e.Id,

                EmployeeName = e.FirstName + " " + e.LastName,

                ActiveTasks = e.AssignedTasks.Count(t =>
                    !t.IsDeleted &&
                    t.EmployeeTaskStatus.Name != "Completed"),

                OverdueTasks = e.AssignedTasks.Count(t =>
                    !t.IsDeleted &&
                    t.DueDate < DateTime.Today &&
                    t.EmployeeTaskStatus.Name != "Completed"),

                DueTodayTasks = e.AssignedTasks.Count(t =>
                    !t.IsDeleted &&
                    t.DueDate.HasValue &&
                    t.DueDate.Value.Date == DateTime.Today &&
                    t.EmployeeTaskStatus.Name != "Completed")
            })
            .ToListAsync();

            foreach (var employee in vm.EmployeesNeedingAttention)
            {
                if (employee.ActiveTasks >= 10)
                {
                    employee.WorkloadLevel = "High";
                    employee.WorkloadColor = "#dc3545";
                }
                else if (employee.ActiveTasks >= 5)
                {
                    employee.WorkloadLevel = "Medium";
                    employee.WorkloadColor = "#fd7e14";
                }
                else
                {
                    employee.WorkloadLevel = "Low";
                    employee.WorkloadColor = "#198754";
                }
            }
            vm.EmployeesNeedingAttention = vm.EmployeesNeedingAttention.Where(x => x.OverdueTasks > 0 || x.ActiveTasks >= 5).OrderByDescending(x => x.OverdueTasks)
                                                .ThenByDescending(x => x.ActiveTasks).Take(5).ToList();
            vm.UpcomingDeadlines = await taskQuery.Where(x => x.CompletedDate == null && x.DueDate.HasValue && x.DueDate.Value.Date >= today)
            .OrderBy(x => x.DueDate)
            .Take(8)
            .Select(x => new UpcomingDeadlineVM
            {
                TaskId = x.Id,

                Title = x.Title,

                AssignedTo = x.AssignedToEmployee != null
                    ? x.AssignedToEmployee.FirstName + " " + x.AssignedToEmployee.LastName
                    : "Unassigned",

                Priority = x.EmployeeTaskPriority.Name,

                PriorityColor = x.EmployeeTaskPriority.ColorCode,

                DueDate = x.DueDate.Value,

                DaysRemaining = EF.Functions.DateDiffDay(today, x.DueDate.Value)
            })
            .ToListAsync();
            //==========================================================
            // Recent Activity
            //==========================================================

            vm.RecentActivities = await _db.TaskActivities
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeTask.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(10)
                .Select(x => new DashboardActivityVM
                {
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,

                    ActivityType = x.ActivityType,

                    Description = x.Description,

                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
            foreach (var task in vm.UpcomingDeadlines)
            {
                if (task.DaysRemaining == 0)
                    task.DueLabel = "Today";
                else if (task.DaysRemaining == 1)
                    task.DueLabel = "Tomorrow";
                else
                    task.DueLabel = $"In {task.DaysRemaining} Days";
            }
            //==========================================================
            // Tasks Requiring Attention
            //==========================================================

            vm.AttentionTasks = await taskQuery
                .Where(x =>
                    (x.CompletedDate == null &&
                     x.DueDate.HasValue &&
                     x.DueDate.Value.Date < today)

                    ||

                    (x.CompletedDate == null &&
                     x.DueDate.HasValue &&
                     x.DueDate.Value.Date == today)

                    ||

                    x.EmployeeTaskPriority.Name == "Critical")
                .OrderBy(x => x.DueDate)
                .ThenByDescending(x => x.EmployeeTaskPriority.DisplayOrder)
                .Take(10)
                .Select(x => new EmployeeTaskSummaryVM
                {
                    Id = x.Id,

                    Title = x.Title,

                    AssignedBy = x.CreatedByEmployee.FirstName + " " + x.CreatedByEmployee.LastName,
                    
                    AssignedTo = x.AssignedToEmployee != null ? x.AssignedToEmployee.FirstName + " " + x.AssignedToEmployee.LastName : "Unassigned",
                    
                    Status = x.EmployeeTaskStatus.Name,

                    StatusColor = x.EmployeeTaskStatus.ColorCode,

                    Priority = x.EmployeeTaskPriority.Name,

                    PriorityColor = x.EmployeeTaskPriority.ColorCode,

                    DueDate = x.DueDate,

                    IsOverdue = x.DueDate.HasValue &&
                                 x.DueDate.Value.Date < today &&
                                 x.CompletedDate == null,
                    DaysRemaining = (DateTime.Now - x.DueDate).Value.Days
                })
                .ToListAsync();
            vm.QuickActions = new List<QuickActionVM>();

            // Company Admin actions
            vm.QuickActions.AddRange(new List<QuickActionVM>
{
    new QuickActionVM
    {
        Title = "Create Task",
        Description = "Assign work to employees",
        Icon = "fa-plus",
        Controller = "EmployeeTask",
        Action = "Create",
        ColorClass = "primary"
    },
    new QuickActionVM
    {
        Title = "Add Employee",
        Description = "Invite new team member",
        Icon = "fa-user-plus",
        Controller = "Employee",
        Action = "Create",
        ColorClass = "success"
    },
    new QuickActionVM
    {
        Title = "Projects",
        Description = "Manage projects",
        Icon = "fa-folder-open",
        Controller = "Project",
        Action = "Index",
        ColorClass = "info"
    },
    new QuickActionVM
    {
        Title = "Reports",
        Description = "View analytics",
        Icon = "fa-chart-column",
        Controller = "Report",
        Action = "Index",
        ColorClass = "warning"
    }
});

            return vm;
        }

        public async Task<SuperAdminDashboardVM> GetSuperAdminDashboardAsync()
        {
            return new SuperAdminDashboardVM();
        }
    }
}
