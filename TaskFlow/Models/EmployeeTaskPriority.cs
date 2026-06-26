namespace TaskFlow.Models
{
    public class EmployeeTaskPriority
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
        public string ColorCode { get; set; } = "#6c757d";
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDefault { get; set; }
        public Company Company { get; set; }

        public ICollection<EmployeeTask> Tasks { get; set; }
    }
}
