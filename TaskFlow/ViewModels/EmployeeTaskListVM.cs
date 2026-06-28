namespace TaskFlow.ViewModels
{
    public class EmployeeTaskListVM
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string AssignedTo { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsOverDue =>
            DueDate.HasValue &&
            DueDate.Value.Date < DateTime.Today;
    }
    public class TaskIndexVM
    {
        public string? SearchText { get; set; }

        public int? EmployeeId { get; set; }

        public int? StatusId { get; set; }

        public int? PriorityId { get; set; }
        public string? DashboardFilter { get; set; }

        public string? DashboardFilterDescription { get; set; }

        public List<EmployeeTaskListVM> Tasks { get; set; }
    }
}
