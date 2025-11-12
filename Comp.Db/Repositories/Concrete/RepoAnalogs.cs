using Comp.Db.Contracts;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class RepoAnalogs : DbRepository<Analog>
{
    public RepoAnalogs(AppDbContext context) : base(context) {
    }

    public override async Task<List<Analog>> GetAllAsync() {
        return await _context.Set<Analog>()
                             .AsNoTracking()
                             .Include(a => a.SourceComponent)
                             .Include(a => a.RelatedComponent)
                             .ToListAsync();
    }

    public override async Task AddAsync(Analog entity) {
        ArgumentNullException.ThrowIfNull(entity.SourceComponent, nameof(entity.SourceComponent));
        ArgumentNullException.ThrowIfNull(entity.RelatedComponent, nameof(entity.RelatedComponent));
        
        var dbEntity = new Analog() {
            Id = default,
            //IsAllCount = entity.IsAllCount, - это надо переносить куда-то ещё
            SourceComponentId = entity.SourceComponent.Id,
            RelatedComponentId = entity.RelatedComponent.Id
        };
        await base.AddAsync(dbEntity);
        
        var dbEntity1 = new Analog() {
            Id = default,
            //IsAllCount = entity.IsAllCount,
            SourceComponentId = entity.RelatedComponent.Id,
            RelatedComponentId = entity.SourceComponent.Id
        };
        await base.AddAsync(dbEntity1);
    }

    public override async Task UpdateAsync(Analog entity) {
        ArgumentNullException.ThrowIfNull(entity.SourceComponent, nameof(entity.SourceComponent));
        ArgumentNullException.ThrowIfNull(entity.RelatedComponent, nameof(entity.RelatedComponent));
        
        if (await GetByIdAsync(entity.Id) is not {} dbEntity)
            throw new KeyNotFoundException();
        
        dbEntity.IsAllCount = entity.IsAllCount;
        dbEntity.SourceComponentId = entity.SourceComponent.Id;
        dbEntity.RelatedComponentId = entity.RelatedComponent.Id;
        
        _context.Entry(dbEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}