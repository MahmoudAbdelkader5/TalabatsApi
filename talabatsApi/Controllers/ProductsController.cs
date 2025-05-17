using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.core.model;
using Talabat.core.Repository;
using Talabat.core.Specifications;
using talabatsApi.DTO;
using talabatsApi.Error;
using talabatsApi.Helper;

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
        public async Task<ActionResult<IReadOnlyList<pagination<ProductDTO>>>> GetProducts([FromQuery]productParam productParam)
        {
            var spec = new productSpecWithbrandAndTypes(productParam);
            var products = await Repo.GetAllAsyncspec(spec);
            if (products == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var productDTOs = mapper.Map<IReadOnlyList<ProductDTO>>(products);

            var countspec = new productSpecificationcount(productParam);
            var counts=await Repo.Productcount(countspec);
            var pagination = new pagination<ProductDTO>
            {
                pageindex = productParam.pageindex,
                Pagesize = productParam.Pagesize,
                count = counts,
                data = productDTOs
            };
            


            return Ok(pagination);
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
        public async Task<ActionResult<IReadOnlyList<ProductType>>> getTypes()
        {
            var types = await productType.GetAllAsync();
            return Ok(types);

        }
        [HttpGet("brand")]
         public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getBrand()
        {
            var productBarnd=await productType.GetAllAsync();
            return Ok(productBarnd);
        }

    }
}
