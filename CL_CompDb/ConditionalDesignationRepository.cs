using CL_Comp_ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace CL_CompDb;

public class ConditionalDesignationRepository : IDisposable
{
    private readonly AppDbContext _context;

    public ConditionalDesignationRepository() {
        _context = new AppDbContext();
        _context.Database.EnsureCreated(); // Создает БД если ее нет
    }

    public List<ConditionalDesignation> GetAll() {
        return _context.ConditionalDesignations.ToList();
    }

    public ConditionalDesignation GetById(int id) {
        return _context.ConditionalDesignations.Find(id);
    }

    public void Add(ConditionalDesignation item) {
        _context.ConditionalDesignations.Add(item);
        _context.SaveChanges();
    }

    public void Update(ConditionalDesignation item) {
        _context.Entry(item).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(int id) {
        var item = _context.ConditionalDesignations.Find(id);
        if (item == null) return;
        _context.ConditionalDesignations.Remove(item);
        _context.SaveChanges();
    }

    public void Dispose() {
        _context?.Dispose();
    }
}