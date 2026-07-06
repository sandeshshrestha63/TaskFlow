using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskFlow.ViewModels.Project
{
    public class ProjectListItemVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public string PriorityName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? TargetEndDate { get; set; }

        public string CreatedByName { get; set; } = string.Empty;
    }

    public class ProjectIndexVM
    {
        public string? Search { get; set; }

        public int? StatusId { get; set; }

        public int? PriorityId { get; set; }

        public List<SelectListItem> Statuses { get; set; } = new();

        public List<SelectListItem> Priorities { get; set; } = new();

        public List<ProjectListItemVM> Projects { get; set; } = new();
    }
}
