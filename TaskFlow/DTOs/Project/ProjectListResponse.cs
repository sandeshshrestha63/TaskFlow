namespace TaskFlow.DTOs.Project
{
    public class ProjectListResponse
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
}
