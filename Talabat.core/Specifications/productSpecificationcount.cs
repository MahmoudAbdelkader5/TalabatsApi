using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model;

namespace Talabat.core.Specifications
{
   public class productSpecificationcount :BaseSpecification<Product>
    {
        public productSpecificationcount(productParam productParam) : base(
            p => (!productParam.producttype.HasValue || p.ProductTypeId == productParam.producttype.Value)
            && (!productParam.productbrand.HasValue || p.ProductBrandId == productParam.productbrand.Value)
            )
        {
        }
    }
}
