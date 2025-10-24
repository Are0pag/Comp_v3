using Comp.ModelData;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class CounterpartyRepository : DbRepository<Counterparty>
{
    public CounterpartyRepository(AppDbContext context) : base(context) {
    }

    public override async Task<List<Counterparty>> GetAllAsync() {
        return await _context.Set<Counterparty>().ToListAsync();
    }

    public override async Task AddAsync(Counterparty entity) {
        ArgumentNullException.ThrowIfNull(entity.ShortName);
        entity.Id = default;
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Counterparty entity) {
        ArgumentNullException.ThrowIfNull(entity.ShortName);
        
        _context.Set<Counterparty>().Attach(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}