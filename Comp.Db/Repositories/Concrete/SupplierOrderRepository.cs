using Comp.Db.Contracts;
using Comp.ModelData;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class SupplierOrderRepository : DbRepository<SupplierOrder>
{
    public SupplierOrderRepository(AppDbContext context) : base(context) {
    }

    public override async Task<List<SupplierOrder>> GetAllAsync() {
        return await _context.Set<SupplierOrder>()
                             //.AsNoTracking()
                             .Include(o => o.Counterparty)
                             .ToListAsync();
    }

    public override async Task AddAsync(SupplierOrder entity) {
        Validate(entity);
        
        entity.Id = default(int);
        _context.Counterparties.Attach(entity.Counterparty);
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(SupplierOrder entity) {
        Validate(entity);

        var trackedEntity = await _context.SupplierOrders
                                          .Include(o => o.Counterparty)
                                          .FirstOrDefaultAsync(o => o.Id == entity.Id);

        if (trackedEntity is null)
            throw new KeyNotFoundException();

        entity.CopyTo(trackedEntity);
        _context.Attach(trackedEntity).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    protected void Validate(SupplierOrder entity) {
        ArgumentNullException.ThrowIfNull(entity.PurchaseOrderNumber);
        ArgumentNullException.ThrowIfNull(entity.Counterparty);
        entity.Counterparty.Id.ThrowIfDefault();
    }
}
