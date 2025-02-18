using AutoMapper;
using BasketSend;
using IdsServer.Database.Models;

namespace IdsServer.Mappings.Profiles;

public class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {

        CreateMap<FakeArticle, typeOrderItem>()
            .ForMember(dest => dest.Kurztext, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Langtext, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ArtNo, opt => opt.MapFrom(src => src.ArticleNumber))
            .ForMember(dest => dest.NetPrice, opt => opt.MapFrom(src => src.NetPrice))
            .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.OfferPrice))
            ;
    }
}