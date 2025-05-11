using AutoMapper;
using Talabat.core.model;
using talabatsApi.DTO;

namespace talabatsApi.Helper
{
    public class profiles : Profile
    {
        public profiles() {

            CreateMap<Product, ProductDTO>().ForMember(d => d.ProductBrandName, o => o.MapFrom(s => s.ProductBrand.Name)).
                 ForMember(d => d.ProductTypeName, o => o.MapFrom(s => s.ProductType.Name)).
                  ForMember(d => d.PictureUrl, o => o.MapFrom<ProductSolverUrl>()); 
        }

    }
}
