namespace TaskFlow.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
