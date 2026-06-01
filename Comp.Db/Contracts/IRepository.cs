using Comp.ModelData.Contracts;

namespace Comp.Db.Contracts;

public interface IRepository<T> 
    where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task<bool> UpdateAsync(int id);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);

    Task<bool> HasAnyUsages<TSet, TItem>(TItem item)
        where TSet : class, IDbEntity
        where TItem : class, IDbEntity;
}