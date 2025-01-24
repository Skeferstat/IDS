using AutoMapper;
using BasketReceive;
using IdsServer.Library.Models;

namespace IdsServer.Mapping;

public class BasketReceiveMappingProfile : Profile
{
    public BasketReceiveMappingProfile()
    {
        CreateMap<typeWarenkorb, BasketDto>()
            .ForMember(dest => dest.BasketId, opt => opt.Ignore())
            .ForMember(dest => dest.BasketInfoDto, opt => opt.MapFrom(src => src.WarenkorbInfo))
            .ForMember(dest => dest.OrderDto, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.RawXml, opt => opt.Ignore())
            .ForMember(dest => dest.HookUrl, opt => opt.Ignore())
            ;

        CreateMap<typeWarenkorbInfo, BasketInfoDto>()
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date) + " " + TimeOnly.FromDateTime(src.Time)))
            ;

        CreateMap<typeOrder, OrderDto>()
            .ForMember(dest => dest.OrderInfoDto, opt => opt.MapFrom(src => src.OrderInfo))
            .ForMember(dest => dest.OrderItemDtos, opt => opt.MapFrom(src => src.OrderItem))
            .AfterMap((src, dest) =>
            {
                foreach (var item in dest.OrderItemDtos)
                {
                    item.Currency = src.OrderInfo.Cur;
                }
            });
        ;

        CreateMap<typeOrderInfo, OrderInfoDto>()
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Cur))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.PartNo))
            ;

        CreateMap<typeOrderItem, OrderItemDto>()
            .ForMember(dest => dest.ArticleNumber, opt => opt.MapFrom(src => src.ArtNo))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Qty))
            .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.OfferPrice))
            .ForMember(dest => dest.NetPrice, opt => opt.MapFrom(src => src.NetPrice))
            .ForMember(dest => dest.PriceBasis, opt => opt.MapFrom(src => src.PriceBasis))
            .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.VAT / 100))
            .ForMember(dest => dest.Supplement, opt => opt.MapFrom(src => src.Zuschlag))
            ;
    }

}