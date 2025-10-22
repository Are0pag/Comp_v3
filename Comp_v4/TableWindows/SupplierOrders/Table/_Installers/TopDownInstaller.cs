using Comp_v4.Installers;
using Comp.Db.Contracts;
using Comp.ModelData;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.SupplierOrders.Table._Installers;

public class TopDownInstaller : ITopDownInstaller
{
    public AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer) {
        if (parentContainer is not RootContainer || childContainer is not SupplierOrderTableContainer)
            throw new ArgumentException();

        //childContainer.FromParentContainer<IRepository<SupplierOrder>>(parentContainer);
        childContainer.SetFactoryMethodFor<IRepository<SupplierOrder>>(() => {
            return parentContainer.Resolve<IRepository<SupplierOrder>>();
        });
        
        

        return childContainer;
    }
}