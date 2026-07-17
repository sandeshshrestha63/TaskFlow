namespace TaskFlow.DTOs.Sprint
{
    public class SprintListResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Goal { get; set; }

        public string SprintStatusName { get; set; } = string.Empty;

        public string BadgeColor { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }

        public decimal ProgressPercentage { get; set; }
    }
}
