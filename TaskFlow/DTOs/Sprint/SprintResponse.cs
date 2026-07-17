namespace TaskFlow.DTOs.Sprint
{
    public class SprintResponse
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Goal { get; set; }

        public string? Description { get; set; }

        public int SprintStatusId { get; set; }

        public string SprintStatusName { get; set; } = string.Empty;

        public string BadgeColor { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }

        public decimal ProgressPercentage { get; set; }
    }
}
