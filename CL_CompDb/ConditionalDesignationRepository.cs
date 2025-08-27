using CL_Comp_ModelData.TechnicalItems;
using CL_CompDb.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CL_CompDb;

public class ConditionalDesignationRepository(AppDbContext context) : DbRepository<ConditionalDesignation>(context)
{
    public override List<ConditionalDesignation> GetAll() {
        return _context.ConditionalDesignations.ToList();
    }

    public override ConditionalDesignation GetById(int id) {
        return _context.ConditionalDesignations.Find(id);
    }

    public override void Add(ConditionalDesignation item) {
        _context.ConditionalDesignations.Add(item);
        _context.SaveChanges();
    }

    public override void Update(ConditionalDesignation item) {
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