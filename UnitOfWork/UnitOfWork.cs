using E_CommerceProductManagementSystem.Data;
using E_CommerceProductManagementSystem.Repositories;
using E_CommerceProductManagementSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_CommerceProductManagementSystem.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceProductManagementSystemDbContext _dbContext;
    
    private IDbContextTransaction _transaction;
    
    // Repository properties exposing data access interfaces
    public ICategoryRepository CategoryRepository { get; }
    
    public IProductRepository ProductRepository { get; }
    
    public ICustomerRepository CustomerRepository { get; }
    
    public IOrderRepository OrderRepository { get; }

    public UnitOfWork(ECommerceProductManagementSystemDbContext dbContext)
    {
        _dbContext = dbContext;
        
        // Initialize repositories with the shared DbContext instance
        CategoryRepository = new CategoryRepository(_dbContext);
        
        ProductRepository = new ProductRepository(_dbContext);
        
        CustomerRepository = new CustomerRepository(_dbContext);
        
        OrderRepository = new OrderRepository(_dbContext);
    }
    
    // Synchronously save all changes tracked by the DbContext to the database
    public int  SaveChanges()
    {
        return _dbContext.SaveChanges();
    }
    
    // Asynchronously save all changes tracked by the DbContext to the database
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    
    // Starts a new transaction asynchronously if none exists; returns current transaction if already active
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_transaction != null)
            return _transaction;
        
        // Begin a new transaction and assign it to _transaction field
        _transaction = await _dbContext.Database.BeginTransactionAsync();
        return _transaction;
    }
    
    // Commits the current transaction asynchronously
    public async Task CommitAsync()
    {
        if (_transaction != null)
            throw new InvalidOperationException("No active transaction to commit");
        
        // First, save any pending changes within the transaction scope
        await _dbContext.SaveChangesAsync();
        
        // Commit the database transaction permanently
        await _transaction.CommitAsync();
        
        // Dispose transaction resources and clear _transaction field
        await DisposeTransactionAsync();
    }
    
    // Rolls back the current transaction asynchronously in case of error
    public async Task RollbackAsync()
    {
        if (_transaction != null)
            throw new  InvalidOperationException("No active transaction to commit");
        
        // Undo all changes made within the transaction scope
        await _transaction.RollbackAsync();
        
        // Dispose transaction resources and clear _transaction field
        await DisposeTransactionAsync();
    }

        
    // Helper method to asynchronously dispose the transaction and clear reference
    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    
    // Dispose method to clean up managed resources
    public void Dispose()
    {
        // Dispose transaction if active
        _transaction?.Dispose();
        
        // Dispose context
        _dbContext.Dispose();
    }
}