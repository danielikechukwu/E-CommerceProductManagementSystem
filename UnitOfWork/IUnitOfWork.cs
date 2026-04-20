using E_CommerceProductManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_CommerceProductManagementSystem.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
   
    // Property to access Order repository for Order entity CRUD operations
    IOrderRepository OrderRepository { get; }
    
    // Property to access Customer repository for Customer entity CRUD operations
    ICustomerRepository CustomerRepository { get; }
    
    // Property to access Product repository for Product entity CRUD operations
    IProductRepository ProductRepository { get; }
    
    // Property to access Category repository for Category entity CRUD operations
    ICategoryRepository CategoryRepository { get; }
    
    // Method to save all changes made in the current unit of work synchronously
    // Returns the number of state entries written to the database
    int SaveChanges();
    
    // Async version of SaveChanges to save changes without blocking the calling thread
    // Returns a Task wrapping the number of state entries written to the database
    Task<int> SaveChangesAsync();
    
    // Starts a new database transaction asynchronously
    // Returns a Task wrapping the transaction object (IDbContextTransaction)
    Task<IDbContextTransaction> BeginTransactionAsync();
    
    // Commits the current database transaction asynchronously
    // Ensures all operations within the transaction are permanently saved
    Task CommitAsync();
    
    // Rolls back the current database transaction asynchronously
    // Reverts all changes made within the transaction in case of errors
    Task RollbackAsync();
}