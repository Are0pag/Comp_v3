using Comp.ModelData;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class CounterpartyRepository : DbRepository<Counterparty>
{
    public CounterpartyRepository(AppDbContext context) : base(context) {
    }

    public override async Task AddAsync(Counterparty entity) {
        ArgumentNullException.ThrowIfNull(entity.ShortName);
        entity.Id = default;
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Counterparty entity) {
        ArgumentNullException.ThrowIfNull(entity.ShortName);
        
        if (await _context.Set<Counterparty>().FirstOrDefaultAsync() is not {} dbInstance)
            throw new KeyNotFoundException();
        
        _context.Entry(dbInstance).CurrentValues.SetValues(entity);
        
        await base.UpdateAsync(entity);
    }
}