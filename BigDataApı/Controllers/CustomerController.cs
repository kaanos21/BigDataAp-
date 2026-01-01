using BigDataApı.Entities;
using BigDataApi.Repositories.Abstract;
using BigDataApı.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
            return Ok("Müşteri başarıyla eklendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerRepository.DeleteAsync(id);
            return Ok("Müşteri silindi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.CustomerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.CustomerSurname = customer.CustomerSurname;
            existingCustomer.CustomerEmail = customer.CustomerEmail;
            existingCustomer.CustomerPhone = customer.CustomerPhone;
            existingCustomer.CustomerImageUrl = customer.CustomerImageUrl;
            existingCustomer.CustomerCountry = customer.CustomerCountry;
            existingCustomer.CustomerCity = customer.CustomerCity;
            existingCustomer.CustomerDistrict = customer.CustomerDistrict;
            existingCustomer.CUstomerAddress = customer.CUstomerAddress;

            await _customerRepository.UpdateAsync(existingCustomer);
            return Ok("Müşteri bilgileri güncellendi.");
        }

        [HttpGet("CustomerListWithPaging")]
        public async Task<IActionResult> GetPagedCustomers(int page, int pageSize)
        {
            var customers = await _customerRepository.CustomerListWithPaging(page, pageSize);
            return Ok(customers);
        }
    }
}