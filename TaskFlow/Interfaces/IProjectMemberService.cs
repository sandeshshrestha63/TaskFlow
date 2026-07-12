using TaskFlow.DTOs.Project;
using TaskFlow.DTOs.ProjectMember;
using TaskFlow.ViewModels.Project;

namespace TaskFlow.Interfaces
{
    public interface IProjectMemberService
    {
        Task<int> AddMemberAsync(ProjectMemberRequest request);
        Task<ProjectMemberFormVM> GetProjectMemberForEditAsync(int projectMemberId);
        Task UpdateProjectMemberAsync(ProjectMemberRequest request);
        Task<ProjectMemberSummaryResponse> GetProjectMemberAsync(int projectMemberId);
        Task RemoveProjectMemberAsync(int projectMemberId);
    }
}
