using AutoMapper;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Models.Support;
using Radzen;

namespace PriceAnalysis.BLL.Transfers;

public class BLLMappingProfile : Profile
{
    public BLLMappingProfile()
    {
        CreateMap<TestEntity, TestDto>();
        CreateMap<SectionEntity, SectionDto>().ReverseMap();
        CreateMap<AlertEntity, NotificationMessage>();
        CreateMap<DocFilesEntity, DocFilesDto>().ReverseMap();
        CreateMap<SpecificationEntity, SpecificationDto>().ReverseMap();
        CreateMap<SupplierEntity, SupplierDto>().ReverseMap();

        CreateMap<KPFilesEntity, KPFilesDto>().ReverseMap();

        CreateMap<SynchPriceEntity, SynchPriceDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

    }
}