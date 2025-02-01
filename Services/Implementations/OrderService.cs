using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace TrendLine.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            return order != null ? _mapper.Map<OrderDTO>(order) : null;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerId(string customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(customerId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task CreateOrder(CreateOrderDTO orderDto, string userId)
        {
            var customer = await _customerRepository.GetCustomerById(userId);
            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found for the given user.");
            }

            var order = new Order
            {
                CustomerId = customer.Id,
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
