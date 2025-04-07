using AutoMapper;
using ModuleProjects.Application.Projects.Commands.Update;
using ModuleProjects.Domain.Projects;
using Newtonsoft.Json;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.Models;

namespace PriceAnalysis.Transfers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<SectionDto, SectionViewModel>()
            .ForMember(dest => dest.CountKPDocFiles, opt => opt.MapFrom(src =>
            src.KPDocFilesId == null ? 0 : JsonConvert.DeserializeObject<List<int>>(src.KPDocFilesId).Count
            ));
        CreateMap<SectionViewModel, SectionDto>();
        CreateMap<SupplierViewModel, SupplierDto>().ReverseMap();
        CreateMap<KPFilesViewModel, KPFilesDto>();
        CreateMap<KPFilesDto, KPFilesViewModel>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

        CreateMap<SpecificationViewModel, SpecificationDto>().ReverseMap();

        //for project passport edit
        CreateMap<Project, UpdateProjectCommand>().ForMember(
            dest => dest.Subcontractors,
            opt => opt.MapFrom(
                src => src.Subcontractors.Select(s =>
                    new ProjectSubcontractorItem()
                    {
                        SubcontractorId = s.Id.SubcontractorId,
                        ProjectResponsibleUserId = s.ProjectResponsibleUserId
                    })));
    }
}