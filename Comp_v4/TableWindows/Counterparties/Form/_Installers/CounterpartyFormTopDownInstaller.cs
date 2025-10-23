using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.Counterparties.Form._Installers;

public class CounterpartyFormTopDownInstaller : ITopDownInstaller
{
    public AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer) {

        childContainer.Add<IRepository<Counterparty>>()
                      .To<CounterpartyRepository>()
                      .AsTransient()
                      .FromParentContainer(parentContainer);
        
        return childContainer;
    }
}