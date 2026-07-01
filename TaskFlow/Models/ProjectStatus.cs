namespace TaskFlow.Models
{
    public class ProjectStatus
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
