using TrendLine.Models;

namespace TrendLine.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);
        Task<IEnumerable<Order>> GetOrdersByStatus(string status);
        Task<IEnumerable<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(int orderId);
    }
}
