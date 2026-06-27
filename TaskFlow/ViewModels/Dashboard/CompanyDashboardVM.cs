using TaskFlow.ViewModels.EmployeeTask;

namespace TaskFlow.ViewModels.Dashboard
{
    public class CompanyDashboardVM
    {
        public int Employees { get; set; }

        public int ActiveTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int OverdueTasks { get; set; }

        public List<EmployeeTaskSummaryVM> RecentTasks { get; set; } = new();

        public List<DashboardActivityVM> RecentActivities { get; set; } = new();
    }
}