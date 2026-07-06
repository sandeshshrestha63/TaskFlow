using AutoMapper;
using TaskFlow.DTOs.Project;
using TaskFlow.Extensions;
using TaskFlow.Models;
using TaskFlow.ViewModels.Project;

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
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByEmployee.GetFullName()));
            CreateMap<ProjectDetailsResponse, ProjectDetailsVM>();
            CreateMap<ProjectListResponse, ProjectListItemVM>();
            CreateMap<ProjectIndexVM, ProjectListRequest>();

            #endregion
        }

    }
}
