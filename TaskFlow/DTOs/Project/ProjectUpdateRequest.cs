namespace TaskFlow.DTOs.Project
{
    public class ProjectUpdateRequest
    {
        public int ProjectId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int ProjectPriorityId { get; set; }
        public int ProjectStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
    }
}
