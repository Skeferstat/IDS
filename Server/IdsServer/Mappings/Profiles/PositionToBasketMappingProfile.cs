using AutoMapper;
using BasketSend;
using IdsServer.Database.Models;
using ImportLibrary;

namespace IdsServer.Mappings.Profiles;

public class PositionToBasketMappingProfile : Profile
{
    public PositionToBasketMappingProfile()
    {

        CreateMap<ArticlePosition, typeOrderItem>()
            .ForMember(dest => dest.ItemChara , opt => opt.MapFrom(src => typeOrderItemItemChara.normal))
            .ForMember(dest => dest.RefItems.Customer, opt => opt.MapFrom(src => src.PosNrHandwerker))
            .ForMember(dest => dest.RefItems.Supplier, opt => opt.MapFrom(src => src.PosNrGH))
            .ForMember(dest => dest.ArtNo, opt => opt.MapFrom(src => src.ArtNr))
            .ForMember(dest => dest.Qty, opt => opt.MapFrom(src => src.Menge))
            .ForMember(dest => dest.Langtext, opt => opt.MapFrom(src => src.Kurztext1))
            .ForMember(dest => dest.Kurztext, opt => opt.MapFrom(src => src.Kurztext2))
            .ForMember(dest => dest.b, opt => opt.MapFrom(src => src.PrBrutto))
            ;
    }
}