using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class EmployeeTaskStatus
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDefault { get; set; }
        public Company Company { get; set; }

        public ICollection<EmployeeTask> Tasks { get; set; }
    }
}
