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
      
      public  productSpecWithbrandAndTypes(int id) : base(p=>p.Id==id)
        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
        }
        public productSpecWithbrandAndTypes(productParam productParam) : base(
            p =>
            (string.IsNullOrEmpty(productParam.Search) || p.Name.ToLower().Contains(productParam.Search))
            && (!productParam.producttype.HasValue || p.ProductTypeId == productParam.producttype.Value)
            && (!productParam.productbrand.HasValue || p.ProductBrandId == productParam.productbrand.Value)
            )

        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
            if(!string.IsNullOrEmpty(productParam.sort))
            {
                switch (productParam.sort)
                {
                    case "price":
                        setOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        setOrderByDescending(p => p.Price);
                        break;
                  
                    case "nameDesc":
                        setOrderByDescending(p => p.Name);
                        break;
                    default:
                        setOrderBy(p => p.Name);
                        break;
                }

               
            }
            // Apply pagination
            int skip = productParam.Pagesize * (productParam.pageindex - 1);
            ApplyPaging(skip, productParam.Pagesize);

        }
        

    }
}
