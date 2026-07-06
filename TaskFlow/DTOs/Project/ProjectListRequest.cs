namespace TaskFlow.DTOs.Project
{
    public class ProjectListRequest
    {
        public int CompanyId { get; set; }

        public string? Search { get; set; }

        public int? StatusId { get; set; }

        public int? PriorityId { get; set; }
    }
}
