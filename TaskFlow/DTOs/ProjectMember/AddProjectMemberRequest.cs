namespace TaskFlow.DTOs.ProjectMember
{
    public class AddProjectMemberRequest
    {
        public int ProjectId { get; set; }

        public int EmployeeId { get; set; }

        public int ProjectRoleId { get; set; }

        public decimal DailyCapacityHours { get; set; }

        public int AllocationPercent { get; set; }

        public bool IsBillable { get; set; }

        public DateTime JoinedDate { get; set; }
    }
}
