namespace TaskFlow.ViewModels.Dashboard
{
    public class DashboardActivityVM
    {
        public string EmployeeName { get; set; } = string.Empty;

        public string ActivityType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public int TaskId { get; set; }

        public string TaskTitle { get; set; } = string.Empty;
    }
}