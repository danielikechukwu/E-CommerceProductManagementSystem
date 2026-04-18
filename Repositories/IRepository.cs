namespace E_CommerceProductManagementSystem.Repositories;

// Generic repository interface defining data access contract for entity type T
// The generic constraint 'where T : class' ensures T is a reference type (entity class)
public interface IRepository<T> where T : class
{
    // Asynchronously retrieves all entities of type T as an enumerable collection
    Task<IEnumerable<T>> GetAllAsync();
    
    // Asynchronously retrieves a single entity by its primary key (int id)
    // Returns null if entity is not found
    Task<T?> GetByIdAsync(int id);
    
    // Asynchronously adds a new entity of type T to the data source
    Task AddAsync(T entity);
    
    // Updates an existing entity of type T in the data source
    void Update(T entity);
    
    // Deletes the specified entity of type T from the data source
    void Delete(T entity);
    
    // Asynchronously checks if an entity with the given id exists in the data source
    // Returns true if found, otherwise false
    Task<bool> ExistsAsync(int id);
    
    // Asynchronously saves all pending changes (add, update, delete) to the database
    Task SaveAsync();
}