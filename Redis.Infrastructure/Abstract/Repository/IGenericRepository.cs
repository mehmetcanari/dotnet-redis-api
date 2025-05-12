namespace Redis.Infrastructure.Abstract.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllEntitiesAsync();
    Task<T> GetEntityByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}