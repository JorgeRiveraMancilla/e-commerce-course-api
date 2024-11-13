using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.DTOs.Baskets;
using e_commerce_course_api.DTOs.Orders;
using e_commerce_course_api.DTOs.Products;
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
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Product.Description)
                )
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Product.Brand))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Product.Type));

            // Order Repository
            CreateMap<Order, OrderDto>()
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<ProductDto, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();

            // Product Repository
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore());
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.PublicId, opt => opt.Ignore());

            // User Repository
            CreateMap<User, UserDto>();

            // Address Repository
            CreateMap<Address, AddressDto>()
                .ReverseMap();
        }
    }
}
