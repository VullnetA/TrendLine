using AutoMapper;
using TrendLine.DTOs;
using TrendLine.LinksResolvers;
using TrendLine.Models;

namespace TrendLine.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Brand, BrandDTO>();
            CreateMap<BrandDTO, Brand>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Color, ColorDTO>();
            CreateMap<ColorDTO, Color>();

            CreateMap<Size, SizeDTO>();
            CreateMap<SizeDTO, Size>();

            CreateMap<Customer, CustomerDTO>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));
            CreateMap<CustomerDTO, Customer>();

            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Name))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size.Label))
            .ForMember(dest => dest.Links, opt => opt.MapFrom<ProductLinksResolver>());
            CreateMap<ProductDTO, Product>();

            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.Links, opt => opt.MapFrom<OrderLinksResolver>())
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderDTO, Order>();

            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemDTO, OrderItem>();
        }
    }
}
