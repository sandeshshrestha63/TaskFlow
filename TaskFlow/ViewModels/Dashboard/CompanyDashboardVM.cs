using TaskFlow.ViewModels.EmployeeTask;

namespace TaskFlow.ViewModels.Dashboard
{
    public class CompanyDashboardVM
    {
        public string CompanyName { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }

        public int ActiveTasks { get; set; }

        public int CompletedToday { get; set; }

        public int OverdueTasks { get; set; }

        public int ActiveProjects { get; set; }

        public TeamInsightsVM TeamInsights { get; set; } = new();        //public TeamInsightsSummaryVM TeamInsights { get; set; } = new();

        public List<EmployeeAttentionVM> EmployeesNeedingAttention { get; set; } = new();
        public List<EmployeeTaskSummaryVM> AttentionTasks { get; set; } = new();
        public List<UpcomingDeadlineVM> UpcomingDeadlines { get; set; } = new();
        public List<DashboardActivityVM> RecentActivities { get; set; } = new();
        public List<QuickActionVM> QuickActions { get; set; } = new();
    }

    public class TeamInsightsVM
    {
        // Employee Statistics
        public int TotalEmployees { get; set; }

        public int BalancedEmployees { get; set; }

        public int BusyEmployees { get; set; }

        public int OverloadedEmployees { get; set; }

        public int EmployeesWithoutTasks { get; set; }

        // Task Statistics
        public int TotalActiveTasks { get; set; }

        public int TasksCompleted { get; set; }

        public decimal AverageTasksPerEmployee { get; set; }

        public decimal CompletionRate { get; set; }

        public decimal AverageCompletionDays { get; set; }
    }
    public class EmployeeAttentionVM
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public int ActiveTasks { get; set; }

        public int OverdueTasks { get; set; }

        public int DueTodayTasks { get; set; }

        public string WorkloadLevel { get; set; } = string.Empty;

        public string WorkloadColor { get; set; } = string.Empty;
    }
    public class TeamInsightsSummaryVM
    {
        public int TasksCompleted { get; set; }

        public int ActiveEmployees { get; set; }

        public double CompletionRate { get; set; }

        public double AverageCompletionDays { get; set; }
    }
    public class UpcomingDeadlineVM
    {
        public long TaskId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string AssignedTo { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string PriorityColor { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public int DaysRemaining { get; set; }

        public string DueLabel { get; set; } = string.Empty;
    }
}