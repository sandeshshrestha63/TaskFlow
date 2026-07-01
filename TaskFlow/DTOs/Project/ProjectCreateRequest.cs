namespace TaskFlow.DTOs.Project
{
    public class ProjectCreateRequest
    {
        public int CompanyId { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int ProjectPriorityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
    }
}
