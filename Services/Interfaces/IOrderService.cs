using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<OrderDTO> GetOrderById(int id);
        Task CreateOrder(CreateOrderDTO orderDto, string userId);
        Task UpdateOrderStatus(string status, int id);
        Task DeleteOrder(int id);
        Task<IEnumerable<OrderDTO>> GetOrdersByStatus(string status);
        Task<IEnumerable<OrderDTO>> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderItemDTO>> GetOrderItemsByOrderId(int orderId);
    }
}
