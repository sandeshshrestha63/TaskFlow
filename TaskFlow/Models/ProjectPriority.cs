namespace TaskFlow.Models
{
    public class ProjectPriority
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string? Color { get; set; }
        public bool IsDefault { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
