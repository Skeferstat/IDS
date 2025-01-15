using AutoMapper;
using IdsServer.Database.Models;
using IdsServer.Library.Models;

namespace IdsServer.Mapping;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        CreateMap<BasketDto, Basket>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasketId))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.RawXml))
            .ForMember(dest => dest.HookUrl, opt => opt.MapFrom(src => src.HookUrl))
            .ForMember(dest => dest.LastUpdate, opt => opt.Ignore())
            ;
    }
}