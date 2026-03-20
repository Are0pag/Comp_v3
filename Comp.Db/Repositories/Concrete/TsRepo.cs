using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class TsRepo : DbRepository<TypeSize>
{
    public TsRepo(AppDbContext context) : base(context) {
    }

    public override async Task AddAsync(TypeSize entity) {
        ArgumentNullException.ThrowIfNull(entity.Designation);
        entity.Id = default;
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(TypeSize entity) {
        ArgumentNullException.ThrowIfNull(entity.Id);
        ArgumentNullException.ThrowIfNull(entity.Designation);

        if (await _context.Set<TypeSize>().FirstOrDefaultAsync(ts => entity.Id == ts.Id) is not { } trackedEntity)
            throw new KeyNotFoundException();
        
        trackedEntity.PopulateFrom(entity);
        _context.Set<TypeSize>().Entry(trackedEntity).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
    }
}