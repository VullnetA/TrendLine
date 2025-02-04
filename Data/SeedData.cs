using TrendLine.Enums;
using TrendLine.Models;

namespace TrendLine.Data
{
    public static class SeedData
    {
        public static readonly List<Brand> Brands = new()
    {
        new Brand { Id = 1, Name = "Adidas" },
        new Brand { Id = 2, Name = "Nike" },
        new Brand { Id = 3, Name = "Zara" },
        new Brand { Id = 4, Name = "Levi's" },
        new Brand { Id = 5, Name = "H&M" }
    };

        public static readonly List<Category> Categories = new()
    {
        new Category { Id = 1, Name = "Casual Wear" },
        new Category { Id = 2, Name = "Formal Wear" },
        new Category { Id = 3, Name = "Sports Wear" },
        new Category { Id = 4, Name = "Outerwear" },
        new Category { Id = 5, Name = "Footwear" }
    };

        public static readonly List<Color> Colors = new()
    {
        new Color { Id = 1, Name = "Denim Blue" },
        new Color { Id = 2, Name = "Heather Gray" },
        new Color { Id = 3, Name = "Olive Green" },
        new Color { Id = 4, Name = "Mustard Yellow" },
        new Color { Id = 5, Name = "Rust" },
        new Color { Id = 6, Name = "Burgundy" },
        new Color { Id = 7, Name = "Navy" },
        new Color { Id = 8, Name = "Dusty Rose" }
    };

        public static readonly List<Size> Sizes = new()
    {
        new Size { Id = 1, Label = "XXS" },
        new Size { Id = 2, Label = "XS" },
        new Size { Id = 3, Label = "S" },
        new Size { Id = 4, Label = "M" },
        new Size { Id = 5, Label = "L" },
        new Size { Id = 6, Label = "XL" },
        new Size { Id = 7, Label = "XXL" },
        new Size { Id = 8, Label = "XXXL" }
    };

        public static readonly List<Product> Products = new()
    {
        new Product
        {
            Id = 100,
            Name = "Classic Denim Jacket",
            Description = "Timeless denim jacket with a comfortable fit and vintage wash",
            Price = 89.99,
            Quantity = 150,
            Gender = Gender.Neutral,
            BrandId = 4,
            CategoryId = 1,
            ColorId = 1,
            SizeId = 4
        },
        new Product
        {
            Id = 101,
            Name = "Performance Running Shoes",
            Description = "Lightweight running shoes with responsive cushioning and breathable mesh",
            Price = 129.99,
            Quantity = 200,
            Gender = Gender.Male,
            BrandId = 2,
            CategoryId = 5,
            ColorId = 2,
            SizeId = 4
        },
        new Product
        {
            Id = 102,
            Name = "Athleisure Hoodie",
            Description = "Comfortable cotton-blend hoodie perfect for workouts or casual wear",
            Price = 65.99,
            Quantity = 175,
            Gender = Gender.Female,
            BrandId = 1,
            CategoryId = 3,
            ColorId = 7,
            SizeId = 5
        },
        new Product
        {
            Id = 103,
            Name = "Business Blazer",
            Description = "Tailored fit blazer suitable for professional settings",
            Price = 149.99,
            Quantity = 100,
            Gender = Gender.Female,
            BrandId = 3,
            CategoryId = 2,
            ColorId = 6,
            SizeId = 3
        },
        new Product
        {
            Id = 104,
            Name = "Winter Parka",
            Description = "Warm and water-resistant parka with faux fur hood",
            Price = 199.99,
            Quantity = 120,
            Gender = Gender.Female,
            BrandId = 5,
            CategoryId = 4,
            ColorId = 3,
            SizeId = 6
        },
        new Product
        {
            Id = 105,
            Name = "Sport Training Pants",
            Description = "Moisture-wicking training pants with zippered pockets",
            Price = 49.99,
            Quantity = 250,
            Gender = Gender.Male,
            BrandId = 1,
            CategoryId = 3,
            ColorId = 2,
            SizeId = 4
        },
        new Product
        {
            Id = 106,
            Name = "Casual Summer Dress",
            Description = "Light and breezy summer dress with floral pattern",
            Price = 79.99,
            Quantity = 150,
            Gender = Gender.Female,
            BrandId = 3,
            CategoryId = 1,
            ColorId = 8,
            SizeId = 3
        },
        new Product
        {
            Id = 107,
            Name = "Classic Sneakers",
            Description = "Versatile everyday sneakers with comfortable insoles",
            Price = 89.99,
            Quantity = 200,
            Gender = Gender.Neutral,
            BrandId = 2,
            CategoryId = 5,
            ColorId = 2,
            SizeId = 5
        },
        new Product
        {
            Id = 108,
            Name = "Formal Trousers",
            Description = "Tailored formal trousers with straight fit",
            Price = 89.99,
            Quantity = 125,
            Gender = Gender.Male,
            BrandId = 3,
            CategoryId = 2,
            ColorId = 7,
            SizeId = 4
        },
        new Product
        {
            Id = 109,
            Name = "Autumn Sweater",
            Description = "Cozy knit sweater perfect for fall weather",
            Price = 59.99,
            Quantity = 175,
            Gender = Gender.Female,
            BrandId = 5,
            CategoryId = 1,
            ColorId = 5,
            SizeId = 4
        }
    };

        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Brands.Any())
            {
                await context.Brands.AddRangeAsync(Brands);
            }

            if (!context.Categories.Any())
            {
                await context.Categories.AddRangeAsync(Categories);
            }

            if (!context.Colors.Any())
            {
                await context.Colors.AddRangeAsync(Colors);
            }

            if (!context.Sizes.Any())
            {
                await context.Sizes.AddRangeAsync(Sizes);
            }

            if (!context.Products.Any())
            {
                await context.Products.AddRangeAsync(Products);
            }

            await context.SaveChangesAsync();
        }
    }
}
