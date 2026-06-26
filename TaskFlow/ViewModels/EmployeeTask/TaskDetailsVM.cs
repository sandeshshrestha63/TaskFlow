namespace TaskFlow.ViewModels.EmployeeTask
{
    public class TaskDetailsVM
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string PriorityColor { get; set; }
        public string StatusColor { get; set; }

        public List<TaskCommentListVM> Comments { get; set; } = new();
        public TaskCommentVM NewComment { get; set; } = new();
        public UpdateTaskStatusVM StatusUpdate { get; set; } = new();
        public List<TaskActivityVM> Activities { get; set; } = new();
    }
    public class TaskCommentListVM
    {
        public string EmployeeName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UpdateTaskStatusVM
    {
        public long TaskId { get; set; }
        public int StatusId { get; set; }
    }
    public class TaskActivityVM
    {
        public string EmployeeName { get; set; }

        public string ActivityType { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
