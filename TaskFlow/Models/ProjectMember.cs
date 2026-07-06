namespace TaskFlow.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int ProjectRoleId { get; set; }
        public ProjectRole ProjectRole { get; set; } = null!;

        public decimal DailyCapacityHours { get; set; }

        public int AllocationPercent { get; set; }

        public bool IsBillable { get; set; }

        public DateTime JoinedDate { get; set; }

        public DateTime? LeftDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
