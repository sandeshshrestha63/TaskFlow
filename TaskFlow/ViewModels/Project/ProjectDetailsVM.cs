using System;
using TaskFlow.ViewModels.Project.Details;

namespace TaskFlow.ViewModels.Project
{
    public class ProjectDetailsVM
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
        
        public int BillableMemberCount => Members.Count(x => x.IsBillable);

        public int NonBillableMemberCount => Members.Count(x => !x.IsBillable);
        public List<ProjectMemberSummaryVM> Members { get; set; } = [];
    }
}