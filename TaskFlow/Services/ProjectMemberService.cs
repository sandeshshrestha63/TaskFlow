using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs.Project;
using TaskFlow.DTOs.ProjectMember;
using TaskFlow.Exceptions;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels.Project;

namespace TaskFlow.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ProjectMemberService(
            AppDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> AddMemberAsync(ProjectMemberRequest request)
        {
            if (request.AllocationPercent is < 0 or > 100)
            {
                throw new ValidationException(
                    "Allocation percentage must be between 0 and 100.");
            }

            if (request.DailyCapacityHours <= 0)
            {
                throw new ValidationException(
                    "Daily capacity hours must be greater than zero.");
            }
            // Validate Project
            var project = await _db.Projects
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ProjectId &&
                    !x.IsDeleted);

            if (project is null)
                throw new ValidationException("The selected project could not be found.");

            // Validate Employee
            var employee = await _db.Employees
                .FirstOrDefaultAsync(x =>
                    x.Id == request.EmployeeId) ?? throw new ValidationException("The selected employee could not be found.");

            if (employee.CompanyId != project.CompanyId)
            {
                throw new ValidationException(
                    "The selected employee does not belong to the project's company.");
            }

            // Validate Role
            var role = await _db.ProjectRoles
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ProjectRoleId &&
                    x.IsActive);

            if (role is null)
                throw new ValidationException("The selected project role could not be found.");

            // Duplicate active member check
            var memberExists = await _db.ProjectMembers.AnyAsync(x =>
                x.ProjectId == request.ProjectId &&
                x.EmployeeId == request.EmployeeId &&
                x.IsActive);

            if (memberExists)
                throw new ValidationException("This employee is already an active member of the project.");

            // Allocation validation
            var currentAllocation = await _db.ProjectMembers
                .Where(x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.IsActive)
                .SumAsync(x => (int?)x.AllocationPercent) ?? 0;

            var totalAllocation = currentAllocation + request.AllocationPercent;

            if (totalAllocation > 100)
            {
                throw new ValidationException(
                    $"This assignment would allocate the employee to {totalAllocation}% across all active projects. The maximum allowed allocation is 100%.");
            }

            var member = _mapper.Map<ProjectMember>(request);

            member.CreatedDate = DateTime.UtcNow;
            member.UpdatedDate = null;
            member.IsActive = true;

            if (member.JoinedDate == default)
            {
                member.JoinedDate = member.CreatedDate;
            }

            _db.ProjectMembers.Add(member);

            await _db.SaveChangesAsync();

            return member.Id;
        }
        public async Task<ProjectMemberFormVM> GetProjectMemberForEditAsync(int projectMemberId)
        {
            var projectMember = await _db.ProjectMembers
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Employee)
                .Include(x => x.ProjectRole)
                .FirstOrDefaultAsync(x =>
                    x.Id == projectMemberId &&
                    x.IsActive);

            if (projectMember == null)
            {
                throw new ValidationException("The requested project member could not be found.");
            }

            return _mapper.Map<ProjectMemberFormVM>(projectMember);
        }

        public async Task UpdateProjectMemberAsync(ProjectMemberRequest request)
        {
            var member = await _db.ProjectMembers
                .FirstOrDefaultAsync(x => x.Id == request.ProjectMemberId && x.IsActive);

            if (member == null)
            {
                throw new ValidationException("The requested project member could not be found.");
            }

            member.ProjectRoleId = request.ProjectRoleId;
            member.DailyCapacityHours = request.DailyCapacityHours;
            member.AllocationPercent = request.AllocationPercent;
            member.IsBillable = request.IsBillable;
            member.JoinedDate = request.JoinedDate;

            await _db.SaveChangesAsync();
        }
        public async Task<ProjectMemberSummaryResponse> GetProjectMemberAsync(int projectMemberId)
        {
            var member = await _db.ProjectMembers
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Employee)
                .Include(x => x.ProjectRole)
                .FirstOrDefaultAsync(x => x.Id == projectMemberId && x.IsActive);

            if (member == null)
            {
                throw new ValidationException("The requested project member could not be found.");
            }

            return _mapper.Map<ProjectMemberSummaryResponse>(member);
        }

        public async Task RemoveProjectMemberAsync(int projectMemberId)
        {
            var member = await _db.ProjectMembers
                .FirstOrDefaultAsync(x => x.Id == projectMemberId && x.IsActive);

            if (member == null)
            {
                throw new ValidationException("The requested project member could not be found.");
            }

            member.IsActive = false;

            await _db.SaveChangesAsync();
        }
    }
}
