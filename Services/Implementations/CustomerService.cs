using AutoMapper;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;

namespace TrendLine.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerById(string customerId)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            return customer != null ? _mapper.Map<CustomerDTO>(customer) : null;
        }

        public async Task AddCustomer(CustomerRegistrationDTO customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _customerRepository.AddCustomer(customer);
        }

        public async Task UpdateCustomer(CustomerDTO customerDto, string customerId)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            if (customer != null)
            {
                _mapper.Map(customerDto, customer);
                await _customerRepository.UpdateCustomer(customer);
            }
        }

        public async Task DeleteCustomer(string customerId)
        {
            await _customerRepository.DeleteCustomer(customerId);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerId(string customerId)
        {
            var orders = await _customerRepository.GetOrdersByCustomerId(customerId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
    }
}
