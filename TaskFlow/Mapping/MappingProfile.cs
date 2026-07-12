using AutoMapper;
using TaskFlow.DTOs.Project;
using TaskFlow.DTOs.ProjectMember;
using TaskFlow.Extensions;
using TaskFlow.Models;
using TaskFlow.ViewModels.Project;
using TaskFlow.ViewModels.Project.Details;

namespace TaskFlow.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            #region Project

            CreateMap<ProjectCreateRequest, Project>();
            CreateMap<ProjectUpdateRequest, Project>();
            CreateMap<ProjectCreateVM, ProjectCreateRequest>();
            CreateMap<Project, ProjectCreateVM>();
            CreateMap<Project, ProjectDetailsResponse>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.ProjectStatus.Name))
                .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.ProjectPriority.Name))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByEmployee.GetFullName()))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ProjectMembers));
            CreateMap<ProjectDetailsResponse, ProjectDetailsVM>();
            CreateMap<ProjectListResponse, ProjectListItemVM>();
            CreateMap<ProjectIndexVM, ProjectListRequest>();

            #endregion

            #region Project Member

            CreateMap<ProjectMemberRequest, ProjectMember>();
            CreateMap<ProjectMemberSummaryResponse, ProjectMemberSummaryVM>();
            CreateMap<ProjectMemberFormVM, ProjectMemberRequest>()
                .ForMember(dest => dest.EmployeeId,
                    opt => opt.MapFrom(src => src.EmployeeId!.Value))
                .ForMember(dest => dest.ProjectRoleId,
                    opt => opt.MapFrom(src => src.ProjectRoleId!.Value));
            CreateMap<ProjectMember, ProjectMemberSummaryResponse>()
                .ForMember(dest => dest.ProjectMemberId,
                    opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.EmployeeId,
                    opt => opt.MapFrom(src => src.EmployeeId))
                
                .ForMember(dest => dest.EmployeeName,
                    opt => opt.MapFrom(src => src.Employee.GetFullName()))

                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Employee.Email))

                .ForMember(dest => dest.ProjectRole,
                    opt => opt.MapFrom(src => src.ProjectRole.Name))

                .ForMember(dest => dest.AllocationPercent,
                    opt => opt.MapFrom(src => src.AllocationPercent))

                .ForMember(dest => dest.DailyCapacityHours,
                    opt => opt.MapFrom(src => src.DailyCapacityHours))

                .ForMember(dest => dest.JoinedDate,
                    opt => opt.MapFrom(src => src.JoinedDate))

                .ForMember(dest => dest.IsBillable,
                    opt => opt.MapFrom(src => src.IsBillable));
            CreateMap<ProjectMember, ProjectMemberFormVM>()
                .ForMember(dest => dest.ProjectMemberId,
                    opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.EmployeeName,
                    opt => opt.MapFrom(src => src.Employee.GetFullName()))

                .ForMember(dest => dest.ProjectName,
                    opt => opt.MapFrom(src => src.Project.Name))

                .ForMember(dest => dest.EmployeeId,
                    opt => opt.MapFrom(src => src.EmployeeId))

                .ForMember(dest => dest.ProjectRoleId,
                    opt => opt.MapFrom(src => src.ProjectRoleId));
            #endregion
        }

    }
}
