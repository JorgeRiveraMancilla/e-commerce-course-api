using AutoMapper;
using e_commerce_course_api.DTOs;
using e_commerce_course_api.Entities;

namespace e_commerce_course_api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Basket, BasketDto>();
            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.ProductId, o => o.MapFrom(s => s.Product.Id))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Product.Name))
                .ForMember(x => x.Price, o => o.MapFrom(s => s.Product.Price))
                .ForMember(x => x.ImageUrl, o => o.MapFrom(s => s.Product.ImageUrl))
                .ForMember(x => x.Brand, o => o.MapFrom(s => s.Product.Brand))
                .ForMember(x => x.Type, o => o.MapFrom(s => s.Product.Type));
            CreateMap<Product, ProductDto>();
            CreateMap<User, UserDto>();
        }
    }
}
