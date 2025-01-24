using AutoMapper;
using BasketReceive;
using IdsServer.Database.Models;
using IdsServer.Library.Models;

namespace IdsServer.Mapping;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        //CreateMap<typeWarenkorb, Basket>()
        //    .ForMember(dest => dest.Id, opt => opt.Ignore())
        //    .ForMember(dest => dest.RawBasket, opt => opt.MapFrom(src => src))
        //    .ForMember(dest => dest.HookUrl, opt => opt.MapFrom(src => src.HookUrl))
        //    .ForMember(dest => dest.LastUpdate, opt => opt.Ignore())
        //    ;
    }
}