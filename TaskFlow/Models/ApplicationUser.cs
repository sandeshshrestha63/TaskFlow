using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
