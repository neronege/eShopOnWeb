using System.Linq;
using AutoMapper;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.PublicApi.CatalogBrandEndpoints;
using Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;
using Microsoft.eShopWeb.PublicApi.CatalogTypeEndpoints;
using Microsoft.eShopWeb.PublicApi.OrderEndpoints;
using Microsoft.eShopWeb.PublicApi.OrderItemEndpoints;
using Microsoft.eShopWeb.PublicApi.OrderListEndpoints;

namespace Microsoft.eShopWeb.PublicApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogItem, CatalogItemDto>();
        CreateMap<CatalogType, CatalogTypeDto>()
            .ForMember(dto => dto.Name, options => options.MapFrom(src => src.Type));
        CreateMap<CatalogBrand, CatalogBrandDto>()
            .ForMember(dto => dto.Name, options => options.MapFrom(src => src.Brand));
        CreateMap<Order, OrderListDTO>()
               .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.BuyerId, opt => opt.MapFrom(src => src.BuyerId))
               .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
               .ForMember(dest => dest.ShipToAddress, opt => opt.MapFrom(src => src.ShipToAddress))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems.Select(item => new OrderItemDTO
                {
                    UnitPrice = item.UnitPrice,
                    Units = item.Units,
                    Id = item.Id
                }).ToList()))
               .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

        CreateMap<Order, OrderDto>();


    }
}
