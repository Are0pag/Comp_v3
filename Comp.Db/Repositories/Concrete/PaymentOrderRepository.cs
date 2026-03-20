using Comp.ModelData;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class PaymentOrderRepository : DbRepository<PaymentOrder>
{
    public PaymentOrderRepository(AppDbContext context) : base(context) {
    }

    public override async Task<List<PaymentOrder>> GetAllAsync() {
        return await _context.Set<PaymentOrder>()
                             .AsNoTracking()
                             .Include(po => po.Order)
                             .ToListAsync();
    }

    public override async Task AddAsync(PaymentOrder entity) {
        Validate(entity);
        
        entity.Id = default;
        entity.Order = _context.Set<SupplierOrder>()
                               .FirstOrDefault(so => so.Id == entity.Order.Id)
                       ?? throw new KeyNotFoundException();
        
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(PaymentOrder entity) {
        Validate(entity);
        entity.Id.ThrowIfDefault();
        entity.OrderId.ThrowIfDefault();
        
        var trackedEntity = _context.Set<PaymentOrder>()
                                    .FirstOrDefault(po => po.Id == entity.Id)
                            ?? throw new KeyNotFoundException();
        
        trackedEntity.PopulateFrom(entity);
        _context.Entry(trackedEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    protected static void Validate(PaymentOrder entity) {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(entity.Order);
        entity.Order.Id.ThrowIfDefault();
    }
}