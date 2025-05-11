using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model;

namespace Talabat.core.Specifications
{
   public class productSpecWithbrandAndTypes  :BaseSpecification<Product>
    {
        public productSpecWithbrandAndTypes():base()
       
        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
        }
      public  productSpecWithbrandAndTypes(int id) : base(p=>p.Id==id)
        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
        }

    }
}
