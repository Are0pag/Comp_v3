/*using Comp_v4.Installers;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
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
        
        selfContainer.Add<IFormHandler>()
                     .To<FormManager>()
                     .AsScoped<SupplierOrderTableWindow>();
        
        selfContainer.Add<AddButVm>()
                     .AsScoped<SupplierOrderTableWindow>();
        
        selfContainer.Add<AddAction>()
                     .AsScoped<SupplierOrderTableWindow>()
                     .EnforceInstantiateOnBegin();

        selfContainer.Add<SupplierOrderTableWindow>()
                     .AsTransient();
        
        return selfContainer;
    }
}*/