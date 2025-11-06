using Comp.ModelData;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class SupplierOrderRepository : DbRepository<SupplierOrder>
{
    public SupplierOrderRepository(AppDbContext context) : base(context) {
    }

    public override async Task<List<SupplierOrder>> GetAllAsync() {
        return await _context.Set<SupplierOrder>()
                             .AsNoTracking()
                             .Include(o => o.Counterparty)
                             .ToListAsync();
    }

    public override async Task AddAsync(SupplierOrder entity) {
        Validate(entity);

        entity.Id = default;
        entity.Counterparty = await _context.Set<Counterparty>()
                                            .FirstOrDefaultAsync(o => o.Id == entity.Counterparty.Id)
                              ?? throw new KeyNotFoundException();
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(SupplierOrder entity) {
        Validate(entity);
        entity.Counterparty.Id.ThrowIfDefault();
        
        var trackedEntity = await _context.Set<SupplierOrder>()
                                          //.Include(o => o.Counterparty)
                                          .FirstOrDefaultAsync(o => o.Id == entity.Id)
                            ?? throw new KeyNotFoundException();


        entity.CopyTo(trackedEntity);
        if (trackedEntity.CounterpartyId != entity.CounterpartyId) {
            
            trackedEntity.CounterpartyId = entity.CounterpartyId;
            trackedEntity.Counterparty = await _context.Set<Counterparty>()
                                                       .FirstOrDefaultAsync(o => o.Id == entity.CounterpartyId)
                                         ?? throw new KeyNotFoundException();
        }
            
        _context.Entry(trackedEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    protected void Validate(SupplierOrder entity) {
        ArgumentNullException.ThrowIfNull(entity.PurchaseOrderNumber);
        ArgumentNullException.ThrowIfNull(entity.Counterparty);
    }
}