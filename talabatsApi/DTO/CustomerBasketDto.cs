using System.ComponentModel.DataAnnotations;
using Talabat.core.model;

namespace talabatsApi.DTO
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
