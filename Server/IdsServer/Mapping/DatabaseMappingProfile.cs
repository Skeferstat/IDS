using AutoMapper;
using BasketReceive;
using IdsServer.Database.Models;
using IdsServer.Library.Models;

namespace IdsServer.Mapping;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        CreateMap<BasketDto, Basket>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.RawXml))
            .ForMember(dest => dest.LastUpdate, opt => opt.Ignore())
            ;
    }
}