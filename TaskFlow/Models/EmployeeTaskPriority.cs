namespace TaskFlow.Models
{
    public class EmployeeTaskPriority
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public Company Company { get; set; }

        public ICollection<EmployeeTask> Tasks { get; set; }
    }
}
