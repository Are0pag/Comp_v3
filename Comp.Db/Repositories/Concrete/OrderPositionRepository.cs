using Comp.ModelData;
using Comp.ModelData.Comp;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class OrderPositionRepository : DbRepository<OrderPosition>
{
    public OrderPositionRepository(AppDbContext context) : base(context) {
    }

    public override async Task<List<OrderPosition>> GetAllAsync() {
        return await _context.Set<OrderPosition>()
                             .AsNoTracking()
                             .Include(op => op.Position)
                             .Include(op => op.SupplierOrder)
                             .ToListAsync();
    }
    
    public async Task<List<OrderPosition>> GetAllBySupplierOrderAsync(int supplierOrderId) {
        var all = await GetAllAsync();
        return all.Where(op => op.SupplierOrderId == supplierOrderId).ToList();
    }
    

    public override async Task AddAsync(OrderPosition entity) {
        Validate(entity);

        entity.Id = default;
        entity.Position = _context.Set<Component>()
                                  .FirstOrDefault(c => c.Id == entity.Position.Id)
                          ?? throw new KeyNotFoundException("Cannot add component to order position.");
        
        entity.SupplierOrderId = entity.SupplierOrder.Id;
        entity.SupplierOrder = null;
        
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(OrderPosition entity) {
        Validate(entity);
        entity.Id.ThrowIfDefault();
        entity.PositionId.ThrowIfDefault();
        
        var trackedEntity = _context.Set<OrderPosition>()
                                                .FirstOrDefault(c => c.Id == entity.Id)
                            ?? throw new KeyNotFoundException("Cannot find component to update order position.");
        
        trackedEntity.PopulateFrom(entity);

        if (trackedEntity.Position is { GenericParametersSet: not null })
            trackedEntity.Position.GenericParametersSet = null;
                                         
        
        _context.Entry(trackedEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    protected static void Validate(OrderPosition entity) {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(entity.Position);
        ArgumentNullException.ThrowIfNull(entity.SupplierOrder);
        entity.Position.Id.ThrowIfDefault();
        entity.SupplierOrder.Id.ThrowIfDefault();
    }
}