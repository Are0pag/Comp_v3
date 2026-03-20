using Comp.Db.Contracts;
using Comp.ModelData;

namespace Comp.Db.Repositories.Concrete;

public static class RepoAnalogsExtensions
{
    public static async Task<int> GetAnalogsCount(this IRepository<Analog> repository, int sourceComponentId) {
        var dbInstances = await repository.GetAllAsync();
        
        if (dbInstances.Count == 0) 
            return 0;
        
        var analogsCount = dbInstances.Count(a => a.SourceComponentId == sourceComponentId);
        return analogsCount;
    }

    public static async Task<IEnumerable<Analog>> GetAnalogsFor(this IRepository<Analog> repository, int sourceComponentId) {
        var dbInstances = await repository.GetAllAsync();
        var analogs = dbInstances.Where(a => a.SourceComponentId == sourceComponentId);
        return analogs;
    }
}