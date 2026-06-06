using AutoMapper;
using BubbleShop.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;
using BubbleShop.Application.Features.Orders.Queries.GetOrderById;
using BubbleShop.Application.Features.Products.Queries.GetAllProducts;
using BubbleShop.Application.Features.Products.Queries.GetProductById;
using BubbleShop.Domain.Entities;

namespace BubbleShop.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductDetailDto>();
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.LineTotal));
        CreateMap<Payment, PaymentDto>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<Delivery, DeliveryDto>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<Customer, CustomerDto>();
    }
}
