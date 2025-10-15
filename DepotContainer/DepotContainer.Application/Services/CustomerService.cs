using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories; // ✅ Dùng interface, không dùng repo cụ thể
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;
namespace DepotContainer.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository; // ✅ Interface repository

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();

            return customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                TaxId = c.TaxId
            });
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                TaxId = customer.TaxId
            };
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                TaxId = dto.TaxId
            };

            await _repository.AddAsync(customer);

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                TaxId = customer.TaxId
            };
        }

        public async Task UpdateAsync(UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            customer.Name = dto.Name;
            customer.TaxId = dto.TaxId;

            await _repository.UpdateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new Exception("Customer not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
