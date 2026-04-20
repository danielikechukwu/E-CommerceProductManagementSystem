using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;
using E_CommerceProductManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProductManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    // private readonly ICustomerRepository _customerRepository;
    private readonly IRepository<Customer> _customerRepository;

    private readonly ILogger<CustomersController> _logger;

    // private readonly IRepository<Customer> _customerRepository;

    public CustomersController(IRepository<Customer> customerRepository, ILogger<CustomersController> logger)
    {
        _customerRepository = customerRepository;

        _logger = logger;
    }

    [HttpGet("GetCustomers")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();

        _logger.LogInformation("Fetching products at {Time}", DateTime.UtcNow);

        var dtos = customers.Select(c => new CustomerDTO()
        {
            CustomerId = c.Id,
            FullName = c.FullName,
            Email = c.Email
        });
        
        return Ok(dtos);
    }

    [HttpGet("GetCustomerById/{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        var dto = new CustomerDTO()
        {
            CustomerId = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email
        };

        return Ok(dto);
    }

    [HttpPost("CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = new Customer()
        {
            FullName = customerDTO.FullName,
            Email = customerDTO.Email
        };

        await _customerRepository.AddAsync(customer);

        await _customerRepository.SaveAsync();

        customerDTO.CustomerId = customer.Id;

        return Ok(customerDTO);
    }

    [HttpPut("UpdateCustomer/{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO dto)
    {
        if (id != dto.CustomerId)
            return BadRequest("Id mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _customerRepository.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        existing.FullName = dto.FullName;
        existing.Email = dto.Email;

        _customerRepository.Update(existing);

        await _customerRepository.SaveAsync();

        return NoContent();
    }

    [HttpDelete("DeleteCustomer/{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var existing = await _customerRepository.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        _customerRepository.Delete(existing);

        await _customerRepository.SaveAsync();

        return NoContent();
    }
}