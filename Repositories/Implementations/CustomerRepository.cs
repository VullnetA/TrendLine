using Microsoft.EntityFrameworkCore;
using TrendLine.Data;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;

namespace TrendLine.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerById(string customerId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<Customer> GetCustomerByTokenId(string customerId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.UserId == customerId);
        }

        public async Task AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(string customerId)
        {
            var customer = await GetCustomerById(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
