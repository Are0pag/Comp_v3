using Comp.Db.Contracts;
using Comp.ModelData;
using Comp.ModelData.Comp;

namespace Comp.Db.Repositories.Concrete;

public class RepoAnalogs : DbRepository<Analog>
{
    protected readonly IRepository<Component> _repositoryComponent;
    
    public RepoAnalogs(AppDbContext context, IRepository<Component> repositoryComponent) : base(context) {
        _repositoryComponent = repositoryComponent;
    }

    public override async Task AddAsync(Analog entity) {
        ArgumentNullException.ThrowIfNull(entity.SourceComponent, nameof(entity.SourceComponent));
        ArgumentNullException.ThrowIfNull(entity.RelatedComponent, nameof(entity.RelatedComponent));
        
        var dbEntity = new Analog() {
            Id = default,
            IsAllCount = entity.IsAllCount,
            SourceComponentId = entity.SourceComponent.Id,
            RelatedComponentId = entity.RelatedComponent.Id
        };
        await base.AddAsync(entity);
    }

    public override async Task<List<Analog>> GetAllAsync() {
        var dbInstances = await base.GetAllAsync();
        foreach (var dbInstance in dbInstances) {
            dbInstance.SourceComponent ??= await _repositoryComponent.GetByIdAsync(dbInstance.SourceComponentId);
            dbInstance.RelatedComponent ??= await _repositoryComponent.GetByIdAsync(dbInstance.RelatedComponentId);
        }
        return dbInstances;
    }

    /*public async Task<int> GetAnalogsCount(int sourceComponentId) {
        var dbInstances = await base.GetAllAsync();
        var analogsCount = dbInstances.Count(a => a.SourceComponentId == sourceComponentId);
        return analogsCount;
    }*/

    public override async Task UpdateAsync(Analog entity) {
        ArgumentNullException.ThrowIfNull(entity.SourceComponent, nameof(entity.SourceComponent));
        ArgumentNullException.ThrowIfNull(entity.RelatedComponent, nameof(entity.RelatedComponent));
        
        if (await GetByIdAsync(entity.Id) is not {} dbEntity)
            throw new KeyNotFoundException();
        
        dbEntity.IsAllCount = entity.IsAllCount;
        dbEntity.SourceComponentId = entity.SourceComponent.Id;
        dbEntity.RelatedComponentId = entity.RelatedComponent.Id;
        
        await base.UpdateAsync(entity);
    }
}