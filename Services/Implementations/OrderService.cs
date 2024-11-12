using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;

namespace TrendLine.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            return order != null ? new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            } : null;
        }

        public async Task CreateOrder(CreateOrderDTO orderDto, string userId)
        {
            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = orderDto.OrderItems.Select(itemDto => new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price
                }).ToList()
            };

            await _orderRepository.AddOrder(order);
        }

        public async Task UpdateOrderStatus(string status, int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order != null)
            {
                order.Status = status;
                await _orderRepository.UpdateOrder(order);
            }
            else
            {
                throw new KeyNotFoundException("Order not found");
            }
        }

        public async Task DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrder(id);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByStatus(string status)
        {
            var orders = await _orderRepository.GetOrdersByStatus(status);
            return orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetOrdersByDateRange(startDate, endDate);
            return orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });
        }

        public async Task<IEnumerable<OrderItemDTO>> GetOrderItemsByOrderId(int orderId)
        {
            var orderItems = await _orderRepository.GetOrderItemsByOrderId(orderId);
            return orderItems.Select(item => new OrderItemDTO
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }
    }
}
