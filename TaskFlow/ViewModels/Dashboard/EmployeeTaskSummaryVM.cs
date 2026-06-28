namespace TaskFlow.ViewModels.Dashboard
{
    public class EmployeeTaskSummaryVM
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string StatusColor { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string PriorityColor { get; set; } = string.Empty;
        public bool IsOverdue { get; set; }
        public string AssignedBy { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int DaysRemaining { get; set; }
    }
}