using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;

namespace E_CommerceProductManagementSystem.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    
    Task<Customer?> GetByIdAsync(int id);
    
    Task AddAsync(Customer customer);
    
    void Update(Customer customer);
    
    void Delete(Customer customer);
    
    Task<bool> ExistsAsync(int id);
    
    Task SaveAsync();
}