using E_CommerceProductManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceProductManagementSystem.Repositories.Implementations;

// GenericRepository class implements IRepository<T> for any class type T
public class GenericRepository<T> : IRepository<T> where T : class
{
    // EF Core DbContext instance for database operations
    private readonly ECommerceProductManagementSystemDbContext _dbContext;
    
    // Private field to hold the DbSet for the specific entity type T
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ECommerceProductManagementSystemDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    
    // Retrieves all entities of type T asynchronously without tracking
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        // AsNoTracking improves performance for read-only queries by disabling change tracking
        // ToListAsync executes the query and returns the results as a list asynchronously
        return await _dbSet.AsNoTracking().ToListAsync();
    }
    
    // Finds an entity by its primary key asynchronously
    public async Task<T?> GetByIdAsync(int id)
    {
        // FindAsync searches the database for an entity with the given primary key
        // Returns null if no entity is found 
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        // AddAsync adds the entity to the DbSet and marks it as Added in the change tracker
        // Actual insertion happens when SaveChangesAsync is called
        await _dbSet.AddAsync(entity);
    }
    
    // Updates an existing entity in the context
    public void Update(T entity)
    {
        // Update marks the entity state as Modified in the change tracker
        // Changes are persisted to the database upon SaveChangesAsync call
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        // Remove marks the entity state as Deleted in the change tracker
        // Entity is removed from the database after SaveChangesAsync is called
        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        // Reuse GetByIdAsync method to try fetching the entity
        var entity = await  _dbSet.FindAsync(id);
        
        // Return true if entity is found, otherwise false
        return entity != null;

    }
    
    // Saves all changes made in the context to the database asynchronously
    public async Task SaveAsync()
    {
        // SaveChangesAsync commits all Insert, Update, Delete operations tracked by DbContext to the database
        await _dbContext.SaveChangesAsync();
    }
}