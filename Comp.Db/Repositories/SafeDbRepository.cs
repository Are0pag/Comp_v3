using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Comp.Db.Repositories;

public class SafeDbRepository<T> : DbRepository<T>
    where T : class
{
    public SafeDbRepository(AppDbContext context) : base(context) {
    }
    
    public override async Task UpdateAsync(T entity) {
        var entry = _context.Entry(entity);
        if (entry.State == EntityState.Detached) {
            var existingEntity = await _context.Set<T>().FindAsync(GetKeyValues(entity));
            if (existingEntity != null) {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else {
                _context.Set<T>().Update(entity);
            }
        }
        else {
            _context.Set<T>().Update(entity);
        }
        await _context.SaveChangesAsync();
    }
    
    public override async Task AddAsync(T entity) {
        // Проверяем, не отслеживается ли уже сущность с таким ключом
        var keyValues = GetKeyValues(entity);
        var existingEntity = await _context.Set<T>().FindAsync(keyValues);
        
        if (existingEntity != null) {
            // Если сущность уже существует, обновляем её вместо добавления
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
        else {
            // Если сущности нет, добавляем новую
            await _context.Set<T>().AddAsync(entity);
        }
        
        await _context.SaveChangesAsync();
    }
    
    public override async Task DeleteAsync(int id) {
        // Создаем временную сущность для удаления
        var entity = Activator.CreateInstance<T>();
        var idProperty = typeof(T).GetProperty("Id");
        
        if (idProperty != null && idProperty.CanWrite) {
            idProperty.SetValue(entity, id);
        }
        
        // Проверяем, отслеживается ли сущность
        var existingEntity = await _context.Set<T>().FindAsync(id);
        
        if (existingEntity != null) {
            // Если сущность отслеживается, удаляем её
            _context.Set<T>().Remove(existingEntity);
            await _context.SaveChangesAsync();
        }
        else {
            // Если сущность не отслеживается, пытаемся присоединить и удалить
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached) {
                _context.Set<T>().Attach(entity);
            }
            _context.Set<T>().Remove(entity);
            
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                // Игнорируем ошибку, если сущность уже была удалена
                Console.WriteLine("Entity was already deleted");
            }
        }
    }
    
    private object[] GetKeyValues(T entity) {
        if (_context.Model.FindEntityType(typeof(T)) is not { } entityType) 
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not implement IEntityType");
        
        if (entityType.FindPrimaryKey() is not { } key)
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a primary key");
        
        var keyValues = new object[key.Properties.Count];
    
        for (int i = 0; i < key.Properties.Count; i++) {
            var property = key.Properties[i];
            
            if (property.PropertyInfo is not { } propertyInfo)
                throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a primary key");
            
            keyValues[i] = propertyInfo.GetValue(entity) ?? throw new InvalidOperationException($"GetValue(entity) is null");
        }
        return keyValues;
    }
    
    // Дополнительный метод для получения ключевых значений по ID
    private object[] GetKeyValues(int id) {
        if (_context.Model.FindEntityType(typeof(T)) is not { } entityType) 
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not implement IEntityType");
        
        if (entityType.FindPrimaryKey() is not { } key)
            throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a primary key");
        
        if (key.Properties.Count != 1)
            throw new InvalidOperationException("This method only works for single-column primary keys");
        
        return new object[] { id };
    }
}