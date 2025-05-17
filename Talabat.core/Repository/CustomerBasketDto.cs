using Talabat.core.model;

namespace Talabat.core.Repository
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }


    }
}