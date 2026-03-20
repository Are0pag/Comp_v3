using Comp.Db.Contracts;
using Comp.ModelData;

namespace Comp.Db.Repositories.Concrete;

public static class RepoOrderPositionsExtensions
{
    public static async Task<IEnumerable<OrderPosition>> GetAllBySupplierOrderAsync(this IRepository<OrderPosition> repository, int supplierOrderId) {
        var all = await repository.GetAllAsync();
        return all.Where(op => op.SupplierOrderId == supplierOrderId).ToList();
    }
}