using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs.Project;
using TaskFlow.Exceptions;
using TaskFlow.Extensions;
using TaskFlow.Interfaces;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ITaskActivityService _activityService;
        private readonly IDateTimeService _dateTimeService;

        public ProjectService(AppDbContext db, IMapper mapper, ITaskActivityService activityService, IDateTimeService dateTimeService)
        {
            _db = db;
            _mapper = mapper;
            _activityService = activityService;
            _dateTimeService = dateTimeService;
        }

        public async Task<List<ProjectListResponse>> GetProjectsAsync(ProjectListRequest request)
        {
            var query = _db.Projects.AsNoTracking()
                                        .Include(x => x.ProjectStatus)
                                        .Include(x => x.ProjectPriority)
                                        .Include(x => x.CreatedByEmployee)
                                            .Where(x => x.CompanyId == request.CompanyId && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x => x.Name.Contains(search));
            }

            if (request.StatusId.HasValue)
            {
                query = query.Where(x => x.ProjectStatusId == request.StatusId.Value);
            }

            if (request.PriorityId.HasValue)
            {
                query = query.Where(x => x.ProjectPriorityId == request.PriorityId.Value);
            }

            var projects = await query.OrderByDescending(x => x.CreatedDate)
                .Select(x => new ProjectListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    StatusName = x.ProjectStatus.Name,
                    PriorityName = x.ProjectPriority.Name,
                    StartDate = x.StartDate,
                    TargetEndDate = x.TargetEndDate,
                    CreatedByName = x.CreatedByEmployee.GetFullName()
                })
                .ToListAsync();

            return projects;
        }
        public async Task<int> CreateProjectAsync(ProjectCreateRequest request)
        {
            // Get the company's default project status
            var defaultStatus = await _db.ProjectStatus.FirstOrDefaultAsync(x => x.IsDefault && x.IsActive);

            if (defaultStatus == null)
            {
                throw new ValidationException("No default project status has been configured.");
            }

            var project = _mapper.Map<Project>(request);

            project.ProjectStatusId = defaultStatus.Id;
            project.CreatedDate = DateTime.UtcNow;
            project.UpdatedDate = null;
            project.IsDeleted = false;

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            return project.Id;
        }

        public async Task<ProjectDetailsResponse> GetProjectDetailsAsync(int projectId)
        {
            var project = await _db.Projects
                .Include(x => x.ProjectStatus)
                .Include(x => x.ProjectPriority)
                .Include(x => x.CreatedByEmployee)
                .FirstOrDefaultAsync(x => x.Id == projectId && !x.IsDeleted);

            if (project == null)
            {
                throw new ValidationException("The requested project could not be found.");
            }

            return _mapper.Map<ProjectDetailsResponse>(project);
        }

        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            var project = await _db.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == projectId &&
                    x.CompanyId == companyId &&
                    !x.IsDeleted);

            if (project == null)
            {
                throw new ValidationException("Project not found.");
            }

            return project;
        }

        public async Task UpdateProjectAsync(ProjectCreateRequest request)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == request.CompanyId && !x.IsDeleted);

            if (project == null)
            {
                throw new ValidationException("Project not found.");
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.ProjectPriorityId = request.ProjectPriorityId;
            project.StartDate = request.StartDate;
            project.TargetEndDate = request.TargetEndDate;
            project.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
        public async Task DeleteProjectAsync(int projectId, int companyId)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.CompanyId == companyId && !x.IsDeleted);

            if (project == null)
            {
                throw new ValidationException("Project not found.");
            }

            project.IsDeleted = true;
            project.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
