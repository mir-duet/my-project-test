using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Domain.Entities;

namespace MyProject.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}
