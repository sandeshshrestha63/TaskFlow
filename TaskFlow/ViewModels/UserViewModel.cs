namespace TaskFlow.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;

        public string? CompanyName { get; set; }
        public int? CompanyId { get; set; }

        public string? Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
