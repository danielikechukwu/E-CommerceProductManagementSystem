using E_CommerceProductManagementSystem.Data;
using E_CommerceProductManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceProductManagementSystem.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly ECommerceProductManagementSystemDbContext _context;
    
    public ProductRepository(ECommerceProductManagementSystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return _context.Products.Include(p => p.Category).AsNoTracking().ToList();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return _context.Products.Any(p => p.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}