using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class TaskActivity
    {
        public long Id { get; set; }
        public int EmployeeTaskId { get; set; }
        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ActivityType { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        // Navigation
        public EmployeeTask EmployeeTask { get; set; }
        public Employee Employee { get; set; }
    }
}
