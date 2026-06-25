using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels
{
    public class EmployeeTaskDataVM
    {
        public long Id { get; set; }

        [Required]
        [StringLength(250)]

        [Display(Name = "Task Title")]
        public string Title { get; set; }
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Assign To")]
        public int AssignedToEmployeeId { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public int EmployeeTaskPriorityId { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Estimated Hours")]
        public decimal? EstimatedHours { get; set; }
    }
}
