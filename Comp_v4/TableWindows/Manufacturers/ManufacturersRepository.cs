using Comp.Db;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.TableWindows.Manufacturers;

public class ManufacturersRepository : DbRepository<Manufacturer>
{
    public ManufacturersRepository(AppDbContext context) : base(context) {
    }
}