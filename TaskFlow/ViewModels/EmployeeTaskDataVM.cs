using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels
{
    public class EmployeeTaskDataVM
    {
        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public int AssignedToEmployeeId { get; set; }

        [Required]
        public int EmployeeTaskPriorityId { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? EstimatedHours { get; set; }
    }
}
