using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryComponent : DbRepository<Component>
{
    public RepositoryComponent(AppDbContext context) : base(context) {
    }

    public override async Task<List<Component>> GetAllAsync() {
        return await _context.Set<Component>()
                             .AsNoTracking()
                             .Include(c => c.Category)
                             .Include(c => c.GenericParametersSet)
                             .Include(c => c.ConditionalDesignation)
                             .Include(c => c.Manufacturer)
                             .Include(c => c.MeasurementUnit)
                             .Include(c => c.TypeSize)
                             .ToListAsync();
    }

    public override async Task AddAsync(Component entity) {
        Validate(entity);

        entity.Id = default;
        entity.Category = _context.Set<Category>()
                                  .FirstOrDefault(c => c.Id == entity.Category.Id)
                          ?? throw new KeyNotFoundException();
        
        if (entity.GenericParametersSet != null)
            entity.GenericParametersSet = _context.Set<GenericParametersSet>()
                                                  .FirstOrDefault(gps => gps.Id == entity.GenericParametersSet.Id)
                                          ?? throw new KeyNotFoundException();

        if (entity.ConditionalDesignation != null)
            entity.ConditionalDesignation = _context.Set<ConditionalDesignation>()
                                                    .FirstOrDefault(g => g.Id == entity.ConditionalDesignation.Id)
                                            ?? throw new KeyNotFoundException();

        if (entity.Manufacturer != null)
            entity.Manufacturer = _context.Set<Manufacturer>()
                                          .FirstOrDefault(m => m.Id == entity.Manufacturer.Id)
                                  ?? throw new KeyNotFoundException();

        if (entity.MeasurementUnit != null)
            entity.MeasurementUnit = _context.Set<MeasurementUnit>()
                                             .FirstOrDefault(mu => mu.Id == entity.MeasurementUnit.Id)
                                     ?? throw new KeyNotFoundException();

        if (entity.TypeSize != null)
            entity.TypeSize = _context.Set<TypeSize>()
                                      .FirstOrDefault(ts => ts.Id == entity.TypeSize.Id)
                              ?? throw new KeyNotFoundException();
        
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Component entity) {
        Validate(entity);
        entity.Id.ThrowIfDefault();
        entity.CategoryId.ThrowIfDefault();

        var trackedEntity = _context.Set<Component>()
                                    .FirstOrDefault(c => c.Id == entity.Id)
                            ?? throw new KeyNotFoundException();

        trackedEntity.PopulateFrom(targetValues: entity);
        _context.Entry(trackedEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    private static void Validate(Component entity) {
        ArgumentException.ThrowIfNullOrEmpty(entity.Name);
        ArgumentException.ThrowIfNullOrEmpty(entity.NomenclatureNumber);
        ArgumentNullException.ThrowIfNull(entity.Category);
    }
}