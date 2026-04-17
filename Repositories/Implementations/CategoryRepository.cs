using E_CommerceProductManagementSystem.Data;
using E_CommerceProductManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceProductManagementSystem.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly ECommerceProductManagementSystemDbContext _context;

    public CategoryRepository(ECommerceProductManagementSystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Customers.AnyAsync(customer => customer.Id == id);
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}