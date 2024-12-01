using AutoMapper;
using Webshop.Search.Application.Features.SearchCategory.Dtos;
using Webshop.Search.Application.Features.SearchProduct.Dtos;
using Webshop.Search.Domain.AggregateRoots;

namespace Webshop.Search.Application.Profiles
{
    /// <summary>
    /// AutoMapper-profil til mapping mellem søgedomæneklasser og DTO'er.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping for kategorier
            CreateMap<Category, SearchCategoryDto>().ReverseMap();

            // Mapping for produkter
            CreateMap<Product, SearchProductDto>().ReverseMap();
        }
    }
}
