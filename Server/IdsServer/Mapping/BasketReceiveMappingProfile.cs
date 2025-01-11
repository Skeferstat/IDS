using AutoMapper;
using BasketReceive;
using IdsServer.Models;

namespace IdsServer.Mapping;

public class BasketReceiveMappingProfile : Profile
{
    public BasketReceiveMappingProfile()
    {
        CreateMap<typeWarenkorb, BasketDto>()
            .ForMember(dest => dest.BasketInfoDto, opt => opt.MapFrom(src => src.WarenkorbInfo))
            .ForMember(dest => dest.OrderDto, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.RawXml, opt => opt.Ignore())
            ;

        CreateMap<typeWarenkorbInfo, BasketInfoDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.Time.ToLocalTime())))
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
            ;
    }
}