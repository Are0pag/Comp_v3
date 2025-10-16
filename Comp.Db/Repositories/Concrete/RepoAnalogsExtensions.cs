using Comp.Db.Contracts;
using Comp.ModelData;

namespace Comp.Db.Repositories.Concrete;

public static class RepoAnalogsExtensions
{
    public static async Task<int> GetAnalogsCount(this IRepository<Analog> repository, int sourceComponentId) {
        var dbInstances = await repository.GetAllAsync();
        var analogsCount = dbInstances.Count(a => a.SourceComponentId == sourceComponentId);
        return analogsCount;
    }
}