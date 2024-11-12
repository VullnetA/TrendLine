using Microsoft.EntityFrameworkCore;
using TrendLine.Data;
using TrendLine.DTOs;
using TrendLine.Enums;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;

namespace TrendLine.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProduct(AddProductDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Gender = productDto.Gender,
                Quantity = productDto.Quantity,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId,
                ColorId = productDto.ColorId,
                SizeId = productDto.SizeId,
                DiscountId = productDto.DiscountId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateProduct(EditProductDTO productDto, int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.CategoryId = productDto.CategoryId;
                product.BrandId = productDto.BrandId;
                product.ColorId = productDto.ColorId;
                product.SizeId = productDto.SizeId;
                product.Gender = productDto.Gender;

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> FindByCategory(string category)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .Where(p => p.Category.Name.Equals(category))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindByBrand(string brand)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .Where(p => p.Brand.Name.Equals(brand))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindByGender(string gender)
        {
            if (!Enum.TryParse(gender, true, out Gender genderEnum))
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .Where(p => p.Gender == genderEnum)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindByPriceRange(double minPrice, double maxPrice)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindBySize(string size)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .Where(p => p.Size.Label.Equals(size))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindByColor(string color)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Discount)
                .Where(p => p.Color.Name.Equals(color))
                .ToListAsync();
        }

        public async Task<long> CountByCategory(string category)
        {
            return await _context.Products
                .CountAsync(p => p.Category.Name.Equals(category));
        }

        public async Task<long> CountByBrand(string brand)
        {
            return await _context.Products
                .CountAsync(p => p.Brand.Name.Equals(brand));
        }

        public async Task<long> CountByAvailability()
        {
            return await _context.Products
                .CountAsync(p => p.Quantity > 0);
        }

        public async Task<long> CountOutOfStock()
        {
            return await _context.Products
                .CountAsync(p => p.Quantity == 0);
        }

        public async Task UpdateQuantity(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProductQuantityDTO> GetProductQuantity(int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return null;

            var totalSold = await _context.OrderItems
                .Where(oi => oi.ProductId == productId)
                .SumAsync(oi => oi.Quantity);

            var productQuantity = new ProductQuantityDTO
            {
                Id = product.Id,
                Name = product.Name,
                InitialQuantity = product.Quantity,
                SoldQuantity = totalSold,
                CurrentQuantity = product.Quantity - totalSold
            };

            return productQuantity;
        }

        public async Task<IEnumerable<Product>> SearchProducts(ProductSearchDTO searchParams)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.Category))
            {
                query = query.Where(p => p.Category.Name == searchParams.Category);
            }

            if (!string.IsNullOrEmpty(searchParams.Gender))
            {
                // Client-side evaluation for Gender filtering
                query = query.AsEnumerable()
                             .Where(p => p.Gender.ToString().Equals(searchParams.Gender, StringComparison.OrdinalIgnoreCase))
                             .AsQueryable(); // Convert back to IQueryable if needed
            }

            if (!string.IsNullOrEmpty(searchParams.Brand))
            {
                query = query.Where(p => p.Brand.Name == searchParams.Brand);
            }

            if (searchParams.PriceMin.HasValue)
            {
                query = query.Where(p => p.Price >= searchParams.PriceMin.Value);
            }

            if (searchParams.PriceMax.HasValue)
            {
                query = query.Where(p => p.Price <= searchParams.PriceMax.Value);
            }

            if (!string.IsNullOrEmpty(searchParams.Size))
            {
                query = query.Where(p => p.Size.Label == searchParams.Size);
            }

            if (!string.IsNullOrEmpty(searchParams.Color))
            {
                query = query.Where(p => p.Color.Name == searchParams.Color);
            }

            if (searchParams.InStock.HasValue)
            {
                query = query.Where(p => searchParams.InStock.Value ? p.Quantity > 0 : p.Quantity == 0);
            }

            // Use synchronous ToList() after client-side evaluation
            return query.ToList();
        }
    }
}
