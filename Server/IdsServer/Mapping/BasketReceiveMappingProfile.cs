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
            ;
        
        CreateMap<typeWarenkorbInfo, BasketInfoDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.Time.ToLocalTime())))
            ;

        CreateMap<typeOrder, OrderDto>()
            .ForMember(dest => dest.OrderInfoDto, opt => opt.MapFrom(src => src.OrderInfo))
            .ForMember(dest => dest.OrderItemDtos, opt => opt.MapFrom(src => src.OrderItem))
            ;

        CreateMap<typeOrderInfo, OrderInfoDto>()
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.PartNo))
            ;

        CreateMap<typeOrderItem, OrderItemDto>()
            .ForMember(dest => dest.ArticleNumber, opt => opt.MapFrom(src => src.ArtNo))
            ;
    }
}