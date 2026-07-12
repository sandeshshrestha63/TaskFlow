using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels.Project
{
    public class ProjectMemberFormVM
    {
        public int ProjectMemberId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an employee.")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select a Project Role.")]
        public int? ProjectRoleId { get; set; }

        public decimal DailyCapacityHours { get; set; }

        public int AllocationPercent { get; set; }

        public bool IsBillable { get; set; } = true;

        public DateTime JoinedDate { get; set; } = DateTime.Today;

        public List<SelectListItem> Employees { get; set; } = new();

        public List<SelectListItem> ProjectRoles { get; set; } = new();
    }
}