using E_CommerceProductManagementSystem.Models;

namespace E_CommerceProductManagementSystem.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();

    Task<Order> GetOrderAsync(int id);

    Task AddOrderAsync(Order order);

    void UpdateOrder(Order order);

    void DeleteOrder(Order order);

    Task<bool> CustomerExistAsync(int id);

    Task SaveAsync();
}