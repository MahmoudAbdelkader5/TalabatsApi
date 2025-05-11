using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.core.model;

namespace talabatRepository.Data
{
   public class dbcontextSeed
    {
        public static async Task SeedAsync(TalabatDbContext context)
        {
            #region seeding productBrands
            if (!context.ProductBrands.Any())
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var brandsDataPath = Path.Combine(basePath, "Data", "dataseed", "brands.json");
                var brandsData = File.ReadAllText(brandsDataPath);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands == null) return;
                foreach (var item in brands)
                {
                    await context.ProductBrands.AddAsync(item);
                }
                await context.SaveChangesAsync();
            }

            #endregion

            #region seeding productTypes
            if (!context.ProductTypes.Any())
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var typesDataPath = Path.Combine(basePath, "Data", "dataseed", "types.json");
                var typesData = File.ReadAllText(typesDataPath);
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types == null) return;
                foreach (var item in types)
                {
                    await context.ProductTypes.AddAsync(item);
                }
                await context.SaveChangesAsync();
            }
            #endregion
            #region seeding products
            if (!context.Products.Any())
            {
               

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var productsDataPath = Path.Combine(basePath, "Data", "dataseed", "products.json");
                var productsData = File.ReadAllText(productsDataPath);
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products == null) return;
                foreach (var item in products)
                {
                    await context.Products.AddAsync(item);
                }
                await context.SaveChangesAsync();
            }
            #endregion

        }

    }
}
