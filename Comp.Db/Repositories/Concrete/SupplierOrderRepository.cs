using Comp.Db.Contracts;
using Comp.ModelData;

namespace Comp.Db.Repositories.Concrete;

public class SupplierOrderRepository : DbRepository<SupplierOrder>
{
    protected readonly IRepository<Counterparty> _counterpartyRepository;
    public SupplierOrderRepository(AppDbContext context, IRepository<Counterparty> counterpartyRepository) : base(context) {
        _counterpartyRepository = counterpartyRepository;
    }

    public override async Task<List<SupplierOrder>> GetAllAsync() {
        var dbInstances= await base.GetAllAsync();
        foreach (var supplierOrder in dbInstances) {
            supplierOrder.Counterparty ??= await _counterpartyRepository.GetByIdAsync(supplierOrder.CounterpartyId);
        }
        return dbInstances;
    }

    public override async Task AddAsync(SupplierOrder entity) {
        var converted = new SupplierOrder() {
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
        await base.AddAsync(converted);
    }
}