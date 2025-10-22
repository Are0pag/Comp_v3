using Comp_v4.Installers;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.SupplierOrders.Table._Installers;

public class SelfInstaller : ISelfLayerInstaller
{
    public AreopagContainer InstallSelf(AreopagContainer selfContainer) {
        if (selfContainer is not SupplierOrderTableContainer)
            throw new Exception("Self Installer only supports SupplierOrderTableContainer");

        selfContainer.Add<IRepository<SupplierOrder>>()
                     .To<SupplierOrderRepository>()
                     .AsTransient();
        
        selfContainer.Add<DataGridVm>()
                     .AsScoped<SupplierOrderTableWindow>();
        
        return selfContainer;
    }
}