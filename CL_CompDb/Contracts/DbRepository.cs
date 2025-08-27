namespace CL_CompDb.Contracts;

public abstract class DbRepository<T> : IDisposable
{
    protected readonly AppDbContext _context;

    public DbRepository(AppDbContext context) {
        _context = context;
        _context.Database.EnsureCreated(); // Создает БД если ее нет
    }

    public abstract List<T> GetAll();
    public abstract T GetById(int id);
    public abstract void Add(T entity);
    public abstract void Update(T entity);
    
    public virtual void Dispose() {
        _context?.Dispose();
    }
}