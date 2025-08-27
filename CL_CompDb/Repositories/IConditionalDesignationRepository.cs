using Comp.ModelData.TechnicalItems;
using CL_CompDb.Contracts;

namespace CL_CompDb;

public interface IConditionalDesignationRepository : IRepository<ConditionalDesignation>
{
    Task<List<ConditionalDesignation>> GetByNameAsync(string name);
    Task<List<ConditionalDesignation>> GetByDesignationAsync(string designation);
}