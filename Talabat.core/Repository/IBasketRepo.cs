using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model;

namespace Talabat.core.Repository
{
    public interface IBasketRepo 
    {
        Task<bool> DeleteBasketAsync(string basketId);
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
    }
}
