using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class Sprint
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int SprintStatusId { get; set; }


        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Goal { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedByEmployeeId { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedByEmployeeId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public int? DeletedByEmployeeId { get; set; }

        public virtual Employee CreatedByEmployee { get; set; } = null!;

        public virtual Employee? UpdatedByEmployee { get; set; }

        public virtual Employee? DeletedByEmployee { get; set; }

        public virtual Project Project { get; set; } = null!;

        public virtual SprintStatus SprintStatus { get; set; } = null!;

        public virtual ICollection<EmployeeTask> Tasks { get; set; } = new List<EmployeeTask>();
    }
}