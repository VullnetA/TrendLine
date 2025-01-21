using TrendLine.DTOs;
using TrendLine.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TrendLine.Models;

namespace TrendLine.GraphQL
{
    public class Query
    {
        private readonly IOrderService _orderService;
        private readonly IDiscountService _discountService;
        private readonly ICustomerService _customerService;

        public Query(IOrderService orderService, IDiscountService discountService, ICustomerService customerService)
        {
            _orderService = orderService;
            _discountService = discountService;
            _customerService = customerService;
        }

        [GraphQLName("getOrders")]
        [Authorize(Roles = "Admin, Advanced User")]
        [GraphQLDescription("Fetches all placed orders.")]
        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                if (orders == null || !orders.Any())
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("No orders found.")
                            .SetCode("NOT_FOUND")
                            .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                            .Build()
                    );
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("An error occurred while fetching orders.")
                        .SetCode("INTERNAL_SERVER_ERROR")
                        .SetExtension("details", ex.Message)
                        .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                        .Build()
                );
            }
        }

        [GraphQLName("getDiscounts")]
        [Authorize(Roles = "Admin")]
        [GraphQLDescription("Fetches all available discounts.")]
        public async Task<IEnumerable<Discount>> GetAllDiscounts()
        {
            try
            {
                var discounts = await _discountService.GetAllDiscounts();
                if (discounts == null || !discounts.Any())
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("No discounts found.")
                            .SetCode("NOT_FOUND")
                            .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                            .Build()
                    );
                }

                return discounts;
            }
            catch (Exception ex)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("An error occurred while fetching discounts.")
                        .SetCode("INTERNAL_SERVER_ERROR")
                        .SetExtension("details", ex.Message)
                        .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                        .Build()
                );
            }
        }

        [GraphQLName("getCustomers")]
        [Authorize(Roles = "Admin")]
        [GraphQLDescription("Fetches all customers.")]
        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomers();
                if (customers == null || !customers.Any())
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("No customers found.")
                            .SetCode("NOT_FOUND")
                            .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                            .Build()
                    );
                }

                return customers;
            }
            catch (Exception ex)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("An error occurred while fetching customers.")
                        .SetCode("INTERNAL_SERVER_ERROR")
                        .SetExtension("details", ex.Message)
                        .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                        .Build()
                );
            }
        }
    }
}
