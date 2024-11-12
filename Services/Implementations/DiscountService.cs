using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;

namespace TrendLine.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscounts()
        {
            return await _discountRepository.GetAllDiscounts();
        }

        public async Task<Discount> GetDiscountById(int id)
        {
            var discount = await _discountRepository.GetDiscountById(id);
            if (discount == null)
            {
                throw new KeyNotFoundException("Discount not found");
            }
            return discount;
        }

        public async Task AddDiscount(AddDiscountDTO discountDto)
        {
            var discount = new Discount
            {
                Name = discountDto.Name,
                DiscountAmount = discountDto.DiscountAmount,
                DiscountPercentage = discountDto.DiscountPercentage,
                ExpirationDate = discountDto.ExpirationDate
            };

            await _discountRepository.AddDiscount(discount);
        }

        public async Task UpdateDiscount(UpdateDiscountDTO discountDto)
        {
            var discount = await _discountRepository.GetDiscountById(discountDto.Id);
            if (discount == null) throw new KeyNotFoundException("Discount not found");

            discount.DiscountAmount = discountDto.DiscountAmount ?? discount.DiscountAmount;
            discount.DiscountPercentage = discountDto.DiscountPercentage ?? discount.DiscountPercentage;
            discount.ExpirationDate = discountDto.ExpirationDate ?? discount.ExpirationDate;

            await _discountRepository.UpdateDiscount(discount);
        }

        public async Task DeleteDiscount(int id)
        {
            await _discountRepository.DeleteDiscount(id);
        }
    }
}
