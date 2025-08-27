using Comp.ModelData.TechnicalItems;

namespace Comp.Db.Contracts;

public interface IConditionalDesignationRepository : IRepository<ConditionalDesignation>
{
    Task<List<ConditionalDesignation>> GetByNameAsync(string name);
    Task<List<ConditionalDesignation>> GetByDesignationAsync(string designation);
}