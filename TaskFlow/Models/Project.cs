namespace TaskFlow.Models
{
    public class Project
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int ProjectStatusId { get; set; }
        public ProjectStatus ProjectStatus { get; set; }

        public int ProjectPriorityId { get; set; }
        public ProjectPriority ProjectPriority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? TargetEndDate { get; set; }

        public int CreatedByEmployeeId { get; set; }
        public Employee CreatedByEmployee { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    }
}
