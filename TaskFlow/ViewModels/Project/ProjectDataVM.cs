using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.ViewModels.Project
{
    public class ProjectCreateVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public int ProjectPriorityId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Target End Date")]
        [DataType(DataType.Date)]
        public DateTime? TargetEndDate { get; set; }

        public List<SelectListItem> ProjectPriorities { get; set; } = new();
    }
}
