namespace TaskFlow.DTOs.Project
{
    public class ProjectMemberSummaryResponse
    {
        public int ProjectMemberId { get; set; }

        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ProjectRole { get; set; } = string.Empty;

        public decimal AllocationPercent { get; set; }

        public decimal DailyCapacityHours { get; set; }

        public DateTime JoinedDate { get; set; }

        public bool IsBillable { get; set; }
    }
}
