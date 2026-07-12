using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.DTOs.Project;
using TaskFlow.DTOs.ProjectMember;
using TaskFlow.Exceptions;
using TaskFlow.Extensions;
using TaskFlow.Interfaces;
using TaskFlow.ViewModels.Project;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.CompanyAccess)]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IProjectMemberService _projectMemberService;
        private readonly IMapper _mapper;
        public ProjectController(ICurrentUserServices userServices, AppDbContext context, IProjectService projectService,IProjectMemberService projectMemberService, IMapper mapper) : base(userServices, context)
        {
            _projectService = projectService;
            _projectMemberService = projectMemberService;
            _mapper = mapper;
        }

        #region Project
        [HttpGet]
        public async Task<IActionResult> Index(ProjectIndexVM vm)
        {
            await LoadProjectDropdowns(vm);

            var request = _mapper.Map<ProjectListRequest>(vm);

            request.CompanyId = CompanyId;

            var projects = await _projectService.GetProjectsAsync(request);

            vm.Projects = _mapper.Map<List<ProjectListItemVM>>(projects);

            return View(vm);
        }
        
        public async Task<IActionResult> Create()
        {
            var vm = new ProjectCreateVM
            {
                StartDate = DateTime.Today
            };

            await LoadDropdowns(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(vm);
                return View(vm);
            }

            var request = _mapper.Map<ProjectCreateRequest>(vm);

            request.CompanyId = CompanyId;
            request.CreatedByEmployeeId = EmployeeId;

            var projectId = await _projectService.CreateProjectAsync(request);

            SuccessMessage("Project created successfully.");

            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _projectService.GetProjectDetailsAsync(id);
            response.TaskCount = 0;
            response.ActiveSprintCount = 0;
            response.ProgressPercentage = 0;
            var vm = _mapper.Map<ProjectDetailsVM>(response);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var companyId = CompanyId;

            var project = await _projectService.GetProjectByIdAsync(id, companyId);

            var vm = _mapper.Map<ProjectCreateVM>(project);

            await LoadDropdowns(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(vm);
                return View(vm);
            }

            var request = _mapper.Map<ProjectCreateRequest>(vm);

            request.CompanyId = CompanyId;

            await _projectService.UpdateProjectAsync(request);

            TempData["Success"] = "Project updated successfully.";

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Archive(int id)
        {
            var companyId = CompanyId;

            var project = await _projectService.GetProjectByIdAsync(id, companyId);

            var vm = _mapper.Map<ProjectCreateVM>(project);

            await LoadDropdowns(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(ProjectCreateVM vm)
        {
            await _projectService.DeleteProjectAsync(vm.Id, CompanyId);

            TempData["Success"] = "Project archived successfully.";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Project Members

        public async Task<IActionResult> AddMember(int projectId)
        {
            var vm = new ProjectMemberFormVM
            {
                ProjectId = projectId,
                DailyCapacityHours = 8,
                AllocationPercent = 100
            };

            await LoadMemberDropdowns(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(ProjectMemberFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadMemberDropdowns(vm);
                return View(vm);
            }

            try
            {
                var request = _mapper.Map<ProjectMemberRequest>(vm);

                await _projectMemberService.AddMemberAsync(request);

                TempData["Success"] = "Project member added successfully.";

                return RedirectToAction(nameof(Details), new { id = vm.ProjectId });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadMemberDropdowns(vm);

                return View(vm);
            }
        }

        public async Task<IActionResult> EditMember(int id)
        {
            try
            {
                var vm = await _projectMemberService.GetProjectMemberForEditAsync(id);

                await LoadMemberDropdowns(vm);

                return View(vm);
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(ProjectMemberFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadMemberDropdowns(vm);
                return View(vm);
            }

            try
            {
                var request = _mapper.Map<ProjectMemberRequest>(vm);

                await _projectMemberService.UpdateProjectMemberAsync(request);

                TempData["Success"] = "Project member updated successfully.";

                return RedirectToAction(nameof(Details), new { id = vm.ProjectId });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadMemberDropdowns(vm);

                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _projectMemberService.GetProjectMemberAsync(id);

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMember(ProjectMemberSummaryResponse model)
        {
            try
            {
                await _projectMemberService.RemoveProjectMemberAsync(model.ProjectMemberId);

                TempData["Success"] = "Project member removed successfully.";

                return RedirectToAction(nameof(Details), new { id = model.ProjectId });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }

        #endregion

        #region Dropdowns
        private async Task LoadProjectDropdowns(ProjectIndexVM vm)
        {
            vm.Statuses = await _db.ProjectStatus.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            vm.Priorities = await _db.ProjectPriorities.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            vm.Statuses.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Statuses"
            });

            vm.Priorities.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Priorities"
            });
        }
        private async Task LoadDropdowns(ProjectCreateVM vm)
        {
            var priorities = await _db.ProjectPriorities.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToListAsync();
            vm.ProjectPriorities.AddRange(priorities);
        }

        private async Task LoadMemberDropdowns(ProjectMemberFormVM vm)
        {
            // Get the project company
            var project = await _db.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == vm.ProjectId &&
                    !x.IsDeleted);

            if (project is null)
            {
                throw new ValidationException("The selected project could not be found.");
            }

            // Only load employees when adding a new member.
            // Employee cannot be changed once assigned to a project.
            if (vm.ProjectMemberId == 0)
            {
                // Get employee IDs already assigned to this project
                var assignedEmployeeIds = await _db.ProjectMembers
                    .AsNoTracking()
                    .Where(x =>
                        x.ProjectId == vm.ProjectId &&
                        x.IsActive)
                    .Select(x => x.EmployeeId)
                    .ToListAsync();

                vm.Employees = await _db.Employees
                    .AsNoTracking()
                    .Where(x =>
                        x.CompanyId == project.CompanyId &&
                        !assignedEmployeeIds.Contains(x.Id))
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.GetFullName()
                    })
                    .ToListAsync();
            }

            // Project Roles
            vm.ProjectRoles = await _db.ProjectRoles
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }
        #endregion
    }
}
