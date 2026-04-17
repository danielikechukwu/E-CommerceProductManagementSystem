using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;

namespace E_CommerceProductManagementSystem.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomerAsync(int id);
    
    Task<Customer> GetCustomerByIdAsync(int id);
    
    Task AddCustomerAsync(CustomerDTO customer);
    
    void Update(CustomerDTO customer);
    
    void DeleteCustomer(int id);
    
    Task<bool> CustomerExistAsync(int id);
    
    Task SaveAsync();
}