using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.model
{
    public class CustomerBasket
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
        public CustomerBasket()
        {
        }
        public CustomerBasket(string id)
        {
            Id = id;
        }
    }
}
