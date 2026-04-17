using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;

namespace E_CommerceProductManagementSystem.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    
    Task<Product?> GetProductById(int id);
    
    Task AddProduct(ProductDTO product);
    
    void UpdateProduct(ProductDTO product);
    
    void DeleteProduct(int id);
    
    Task<bool> ExistAsync(int id);
    
    Task SaveAsync();
    
}