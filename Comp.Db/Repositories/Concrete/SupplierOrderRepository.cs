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
                             .AsNoTracking()
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
        
        if (await _context.SupplierOrders.FirstOrDefaultAsync() is not {} dbEntity)
            throw new NullReferenceException("Counterparties could not be found");
        
        _context.Entry(dbEntity).CurrentValues.SetValues(entity);
        dbEntity.CounterpartyId = entity.Counterparty.Id;
        
        await base.UpdateAsync(entity);
    }

    protected void Validate(SupplierOrder entity) {
        ArgumentNullException.ThrowIfNull(entity.PurchaseOrderNumber);
        ArgumentNullException.ThrowIfNull(entity.Counterparty);
        entity.Counterparty.Id.ThrowIfDefault();
    }
}

/*var dbInstances= await base.GetAllAsync();
foreach (var supplierOrder in dbInstances) {
    supplierOrder.Counterparty ??= await _counterpartyRepository.GetByIdAsync(supplierOrder.CounterpartyId);
}
return dbInstances;*/


/*var converted = new SupplierOrder() {
    Id = default,
    CounterpartyId = entity.Counterparty.Id,

    PurchaseOrderNumber = entity.PurchaseOrderNumber,
    InvoiceNumber = entity.InvoiceNumber,
    Note = entity.Note,

    OrderStatus = entity.OrderStatus,
    VatStatus = entity.VatStatus,

    ContractFilePath = entity.ContractFilePath,
    InvoiceFilePath = entity.InvoiceFilePath,

    OrderDate = entity.OrderDate,
    DeliveryDate = entity.DeliveryDate,
};
await base.AddAsync(converted);*/