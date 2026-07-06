using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.DTOs.Project;
using TaskFlow.Interfaces;
using TaskFlow.ViewModels.Project;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.CompanyAccess)]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        public ProjectController(ICurrentUserServices userServices, AppDbContext context, IProjectService projectService, IMapper mapper) : base(userServices, context)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

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
            response.MemberCount = 0;
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
    }
}
