using System.ComponentModel.DataAnnotations;
using TaskFlow.Helpers;

namespace TaskFlow.Models
{
    public class EmployeeTask
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }
        public int? ProjectId { get; set; }

        public int? SprintId { get; set; }

        public int CreatedByEmployeeId { get; set; }

        public int? AssignedToEmployeeId { get; set; }

        public int EmployeeTaskStatusId { get; set; }

        public int EmployeeTaskPriorityId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public decimal? EstimatedHours { get; set; }

        public decimal? ActualHours { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation

        public Company Company { get; set; }

        public Employee CreatedByEmployee { get; set; }

        public Employee AssignedToEmployee { get; set; }

        public EmployeeTaskStatus EmployeeTaskStatus { get; set; }

        public EmployeeTaskPriority EmployeeTaskPriority { get; set; }

        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
        public ICollection<TaskActivity> Activities { get; set; } = new List<TaskActivity>();
        public ICollection<EmployeeTaskAttachment> Attachments { get; set; } = new List<EmployeeTaskAttachment>();
    }
}
