using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.GraphQL.Resolvers
{
    public class CustomerResolvers
{
    public async Task<IEnumerable<OrderDTO>> GetOrders([Parent] CustomerDTO customer, [Service] IOrderService orderService)
    {
            if (string.IsNullOrEmpty(customer.UserId)) return null;

            var orders = await orderService.GetOrdersByCustomerId(customer.Id);
            return orders ?? null;           
    }
}
}
