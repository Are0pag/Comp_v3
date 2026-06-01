using Comp.Db.Contracts;
using Comp.ModelData.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories;

public class DbRepository<T> : IRepository<T> 
    where T : class
{
    protected readonly AppDbContext _context;

    public DbRepository(AppDbContext context) {
        _context = context;
    }

    public virtual async Task<List<T>> GetAllAsync() {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id) {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task AddAsync(T entity) {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task UpdateAsync(T entity) {
        try {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public virtual async Task<bool> UpdateAsync(int id) {
        try {
            if (await GetByIdAsync(id) is not { } entity) 
                return false;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        return true;
    }

    public virtual async Task DeleteAsync(int id) {
        try {
            var entity = await GetByIdAsync(id);
            if (entity != null) {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e) {
            throw;
        }
    }

    public virtual async Task<bool> HasAnyUsages<TSet, TItem>(TItem item) 
        where TSet : class, IDbEntity
        where TItem : class, IDbEntity
    {
        var entity = await GetByIdAsync(item.Id);
        return await _context.Set<TSet>()
                             .AnyAsync(c => EF.Property<TItem>(c, typeof(TItem).Name) == entity);
    }
}