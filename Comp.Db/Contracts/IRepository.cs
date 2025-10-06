namespace Comp.Db.Contracts;

public interface IRepository<T> 
    where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<bool> AddAsync(int id);
    Task AddAsync(T entity);
    Task<bool> UpdateAsync(int id);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}