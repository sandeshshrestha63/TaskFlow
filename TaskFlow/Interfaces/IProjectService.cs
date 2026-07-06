using TaskFlow.DTOs.Project;
using TaskFlow.Models;

namespace TaskFlow.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectListResponse>> GetProjectsAsync(ProjectListRequest request);
        Task<int> CreateProjectAsync(ProjectCreateRequest request);
        Task<ProjectDetailsResponse> GetProjectDetailsAsync(int projectId);
        Task<Project> GetProjectByIdAsync(int projectId, int companyId);
        Task UpdateProjectAsync(ProjectCreateRequest request);
        Task DeleteProjectAsync(int projectId, int companyId);
    }
}
