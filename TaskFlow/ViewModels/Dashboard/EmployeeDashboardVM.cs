using TaskFlow.ViewModels.EmployeeTask;

namespace TaskFlow.ViewModels.Dashboard
{
    public class EmployeeDashboardVM
    {
        public int MyTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int DueToday { get; set; }
        public int OverdueTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedToday { get; set; }

        public int TotalDueToday { get; set; }

        public int CompletionPercentage { get; set; }
        public EmployeeTaskSummaryVM? FocusTask { get; set; }
        public List<EmployeeTaskSummaryVM> UpcomingTasks { get; set; } = new();
        public List<EmployeeTaskSummaryVM> MyTaskList { get; set; } = new();
        public List<DashboardActivityVM> RecentActivities { get; set; } = new();
    }
}