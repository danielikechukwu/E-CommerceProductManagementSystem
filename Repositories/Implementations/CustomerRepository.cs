using E_CommerceProductManagementSystem.Data;
using E_CommerceProductManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceProductManagementSystem.Repositories.Implementations;

public class CustomerRepository : ICustomerRepository
{
    private readonly ECommerceProductManagementSystemDbContext _context;
    
    public CustomerRepository(ECommerceProductManagementSystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.AsNoTracking().ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}