using E_CommerceProductManagementSystem.Data;
using E_CommerceProductManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceProductManagementSystem.Repositories.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly ECommerceProductManagementSystemDbContext  _context;

    public OrderRepository(ECommerceProductManagementSystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .AsNoTracking().ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }

    public void Delete(Order order)
    {
        _context.Orders.Remove(order);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(o => o.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}