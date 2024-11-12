using Microsoft.EntityFrameworkCore;
using TrendLine.Data;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;

namespace TrendLine.Repositories.Implementations
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _context;

        public DiscountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscounts()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscount(Discount discount)
        {
            _context.Entry(discount).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscount(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Discount> GetDiscountById(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }
    }

}
