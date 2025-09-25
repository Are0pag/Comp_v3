using Comp.Db;
using Comp.Db.Repositories;

namespace Comp_v4.TableWindows.ConditionalDesignation;

public class ConditionalDesignationRepository : DbRepository<Comp.ModelData.TechnicalItems.ConditionalDesignation>
{
    public ConditionalDesignationRepository(AppDbContext context) : base(context) {
    }
}