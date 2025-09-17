using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.Operations.Commands;

public class FactoryDbComm
{
    protected readonly IRepository<ConditionalDesignation> _scopedRepository;

    public FactoryDbComm(IRepository<ConditionalDesignation> scopedRepository) {
        _scopedRepository = scopedRepository;
    }
}