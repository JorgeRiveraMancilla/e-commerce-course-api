using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.DTOs.Orders;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Entities.Baskets;
using e_commerce_course_api.Entities.Orders;

namespace e_commerce_course_api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Address Repository
            CreateMap<Address, AddressDto>();

            // Basket Repository
            CreateMap<Basket, BasketDto>();
            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.ProductId, o => o.MapFrom(s => s.Product.Id))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Product.Name))
                .ForMember(x => x.Description, o => o.MapFrom(s => s.Product.Description))
                .ForMember(x => x.Price, o => o.MapFrom(s => s.Product.Price))
                .ForMember(x => x.ImageUrl, o => o.MapFrom(s => s.Product.ImageUrl))
                .ForMember(x => x.Brand, o => o.MapFrom(s => s.Product.Brand))
                .ForMember(x => x.Type, o => o.MapFrom(s => s.Product.Type));

            // Order Repository
            CreateMap<Order, OrderDto>()
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<ProductDto, OrderItemDto>()
                .ForMember(x => x.ProductId, o => o.MapFrom(s => s.Id));

            // Product Repository
            CreateMap<Product, ProductDto>();

            // User Repository
            CreateMap<User, UserDto>();

            // Address Repository
            CreateMap<Address, AddressDto>()
                .ReverseMap();
        }
    }
}
