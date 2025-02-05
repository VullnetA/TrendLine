using TrendLine.Models;

namespace TrendLine.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(string customerId);
        Task<Customer> GetCustomerByTokenId(string customerId);
        Task AddCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(string customerId);
        Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId);
    }
}
