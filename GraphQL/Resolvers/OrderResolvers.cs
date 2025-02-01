using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.GraphQL.Resolvers
{
    public class OrderResolvers
    {
        public async Task<CustomerDTO?> GetCustomer([Parent] OrderDTO order, [Service] ICustomerService customerService)
        {
            if (string.IsNullOrEmpty(order.CustomerId)) return null;

            var customer = await customerService.GetCustomerById(order.CustomerId);
            return customer ?? null;
        }
    }
}
