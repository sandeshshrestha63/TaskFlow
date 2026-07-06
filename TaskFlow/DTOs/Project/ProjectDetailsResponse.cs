namespace TaskFlow.DTOs.Project
{
    public class ProjectDetailsResponse
    {
        public long ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public string PriorityName { get; set; } = string.Empty;
        public int MemberCount { get; set; }

        public int TaskCount { get; set; }

        public int ActiveSprintCount { get; set; }

        public decimal ProgressPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? TargetEndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedByName { get; set; } = string.Empty;

    }
}
