using Comp.Db;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.Entities;

public class ConditionalDesignationRepository : DbRepository<ConditionalDesignation>
{
    public ConditionalDesignationRepository(AppDbContext context) : base(context) {
    }
}