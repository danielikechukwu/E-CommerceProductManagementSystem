using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;

namespace E_CommerceProductManagementSystem.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    
    Task<Category?> GetByIdAsync(int id);
    
    Task AddAsync(Category category);
    
    void Update(Category category);
    
    void Delete(Category category);
    
    Task<bool> ExistsAsync(int id);
    
    Task SaveAsync();
}