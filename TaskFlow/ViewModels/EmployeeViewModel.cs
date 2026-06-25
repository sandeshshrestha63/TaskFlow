using System.ComponentModel.DataAnnotations;
using TaskFlow.Models;

namespace TaskFlow.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;

        //[Phone(ErrorMessage = "Invalid phone number")]
        //public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select a company")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid company selected")]
        public int CompanyId { get; set; }

        public List<Company> Companies { get; set; } = new List<Company>();
    }
}
