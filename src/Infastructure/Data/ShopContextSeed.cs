using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Data
{
    public static class ShopContextSeed
    {
        public static async Task SeedAsync(ShopContext db)
        {
            await db.Database.MigrateAsync();
            if (await db.Categories.AnyAsync() || await db.Products.AnyAsync() || await db.Brands.AnyAsync())
                return;

            var c1 = new Category() { Name = "Men" };
            var c2 = new Category() { Name = "Women" };
            var c3 = new Category() { Name = "Unisex" };

            var b1 = new Brand() { Name = "Calvin Klein" };
            var b2 = new Brand() { Name = "Tom Ford" };
            var b3 = new Brand() { Name = "Dolce & Gabbana" };
            var b4 = new Brand() { Name = "Chanel" };

            db.Products.AddRange(
                new Product() { Category = c2, Brand = b1, Price = 049.15m, PictureUri = "01.webp", Name = "Eternity 3.4 oz Eua De Parfum Spray" },
                new Product() { Category = c2, Brand = b1, Price = 034.09m, PictureUri = "02.webp", Name = "Obession 3.4 oz Eau De Parfum Spray" },
                new Product() { Category = c1, Brand = b1, Price = 030.12m, PictureUri = "03.webp", Name = "Eternity Aqua 3.4 oz Eau De Toilette Spray" },
                new Product() { Category = c1, Brand = b1, Price = 029.33m, PictureUri = "04.webp", Name = "Ck Free 3.4 oz Eau De Toilette Spray" },
                new Product() { Category = c3, Brand = b1, Price = 030.12m, PictureUri = "05.webp", Name = "Ck Be Perfume 6.6 oz Eau De Toilette Spray" },
                new Product() { Category = c3, Brand = b1, Price = 074.77m, PictureUri = "06.webp", Name = "Ck Everyone 6.7 oz Eau De Parfum Spray" },
                new Product() { Category = c3, Brand = b2, Price = 243.09m, PictureUri = "07.webp", Name = "Tom Ford Ombre Leather 3.4 oz Eau De Parfum Spray" },
                new Product() { Category = c1, Brand = b2, Price = 130.89m, PictureUri = "08.webp", Name = "Tom Ford Noir Extreme 1.7 oz Eau De Parfum Spray" },
                new Product() { Category = c2, Brand = b2, Price = 134.79m, PictureUri = "09.webp", Name = "Black Orchid 1.7 oz Eau De Parfum Spray" },
                new Product() { Category = c1, Brand = b3, Price = 063.43m, PictureUri = "10.webp", Name = "Dolce & Gabbana Intenso 4.2 oz Eau De Parfum Spray" },
                new Product() { Category = c2, Brand = b3, Price = 038.06m, PictureUri = "11.webp", Name = "Light Blue 0.8oz Eau De Toilette Spray" },
                new Product() { Category = c1, Brand = b4, Price = 186.99m, PictureUri = "12.webp", Name = "Bleu De Chanel 3.4 oz Eau De Parfum Spray" }

                );
            await db.SaveChangesAsync();
        }
    }
}
