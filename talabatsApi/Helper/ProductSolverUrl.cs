using AutoMapper;
using System.Threading.Tasks.Sources;
using Talabat.core.model;
using talabatsApi.DTO;

namespace talabatsApi.Helper
{
    public class ProductSolverUrl : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration configuration;

        public ProductSolverUrl(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration["ApiUrl"]}{source.PictureUrl}";
            }
            return string.Empty;

        }
    }
}
