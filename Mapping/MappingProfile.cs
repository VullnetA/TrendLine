using AutoMapper;
using TrendLine.DTOs;
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
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<CustomerDTO, Customer>();

            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Name))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size.Label));
            CreateMap<ProductDTO, Product>();
        }
    }
}
