using Comp_v4.Installers;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.Counterparties.Table._Installers;

public class TopDownInstaller : ITopDownInstaller
{
    public AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer) {
        if (parentContainer is not RootContainer || childContainer is not CounterpartyTableContainer)
            throw new ArgumentException();
        
        childContainer.Add<CounterpartyFormContainer>()
                      .AsScoped<CounterpartyTableWindow>()
                      .FromParentContainer(parentContainer);
        
        childContainer.Add<IRepository<Counterparty>>()
                      .To<CounterpartyRepository>()
                      .AsTransient()
                      .FromParentContainer(parentContainer);
        
        return childContainer;
    }
}