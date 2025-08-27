using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp.Db.Repositories;

public interface IConditionalDesignationRepository : IRepository<ConditionalDesignation>
{
    Task<List<ConditionalDesignation>> GetByNameAsync(string name);
    Task<List<ConditionalDesignation>> GetByDesignationAsync(string designation);
}