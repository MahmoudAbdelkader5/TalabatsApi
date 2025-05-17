using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.model;
using Talabat.core.Repository;
using talabatsApi.Error;

namespace talabatsApi.Controllers
{

    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper mapper;

        public BasketController(IBasketRepo basketRepo , IMapper mapper)
        {
            _basketRepo = basketRepo;
            this.mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
      
            var basket = await _basketRepo.GetBasketAsync(id);
            if (basket == null)
                return (new CustomerBasket(id));
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basketDto)
        {

            var updatedBasket = await _basketRepo.UpdateBasketAsync(basketDto);

            if (updatedBasket == null)
                return BadRequest(new ApiResponse(400));

            return Ok(updatedBasket);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
           return await _basketRepo.DeleteBasketAsync(id);
        }
    }
    
}
