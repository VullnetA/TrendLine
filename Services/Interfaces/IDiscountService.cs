using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task<Discount> GetDiscountById(int id);
        Task AddDiscount(AddDiscountDTO discountDto);
        Task UpdateDiscount(UpdateDiscountDTO discountDto);
        Task DeleteDiscount(int id);
    }
}
