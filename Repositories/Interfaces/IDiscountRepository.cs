using TrendLine.Models;

namespace TrendLine.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task AddDiscount(Discount discount);
        Task UpdateDiscount(Discount discount);
        Task DeleteDiscount(int id);
        Task<Discount> GetDiscountById(int id);
    }
}
