using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.core.model;
using Talabat.core.Repository;
using Talabat.core.Specifications;
using talabatsApi.DTO;
using talabatsApi.Error;

namespace talabatsApi.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        private readonly IMapper mapper;
        private readonly IgerenicRepo<ProductType> productType;
        private readonly IgerenicRepo<ProductBrand> productBarnd;

        public IgerenicRepo<Product> Repo { get; }

        public ProductsController(IgerenicRepo<Product> repo , IMapper mapper,IgerenicRepo<ProductType> productType ,IgerenicRepo<ProductBrand> productBarnd )
        {
            Repo = repo;
            this.mapper = mapper;
            this.productType = productType;
            this.productBarnd = productBarnd;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var spec = new productSpecWithbrandAndTypes();
            var products = await Repo.GetAllAsyncspec(spec);
            if (products == null)
            {
                return NotFound(new ApiResponse(404));
            }

            #region Manual mapping
            //// Manual mapping of Product to ProductDTO
            //var productDTOs = products.Select(product => new ProductDTO
            //{
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    ProductBrandId = product.ProductBrandId,
            //    ProductTypeId = product.ProductTypeId,
            //    PictureUrl = product.PictureUrl,
            //    ProductBrandName = product.ProductBrand.Name,
            //    ProductTypeName = product.ProductType.Name
            //}).ToList();
            #endregion
            var productDTOs = mapper.Map<IEnumerable<ProductDTO>>(products);
            


            return Ok(productDTOs);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            // Create a specification with the criteria to match the product by ID
            var spec = new productSpecWithbrandAndTypes(id);
            var product = await Repo.GetByIdAsyncspec(spec);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var productDTO = mapper.Map<ProductDTO>(product);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(productDTO);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> getTypes()
        {
            var types = await productType.GetAllAsync();
            return Ok(types);

        }
        [HttpGet("brand")]
         public async Task<ActionResult<IEnumerable<ProductBrand>>> getBrand()
        {
            var productBarnd=await productType.GetAllAsync();
            return Ok(productBarnd);
        }

    }
}
