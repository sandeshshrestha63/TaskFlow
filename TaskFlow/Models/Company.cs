namespace TaskFlow.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
