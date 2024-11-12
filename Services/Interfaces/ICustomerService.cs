using TrendLine.DTOs;

namespace TrendLine.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomers();
        Task<CustomerDTO> GetCustomerById(string customerId);
        Task AddCustomer(CustomerRegistrationDTO customerDto);
        Task UpdateCustomer(CustomerDTO customerDto, string customerId);
        Task DeleteCustomer(string customerId);
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomerId(string customerId);
    }
}
