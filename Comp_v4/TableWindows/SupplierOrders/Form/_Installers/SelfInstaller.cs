using Comp_v4.Installers;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.SupplierOrders.Form._Installers;

public class SelfInstaller : ISelfLayerInstaller
{
    public AreopagContainer InstallSelf(AreopagContainer selfContainer) {
        if (selfContainer is not SupplierOrderFormContainer)
            throw new Exception("Self Installer only supports SupplierOrderFormContainer");

        selfContainer.Add<IRepository<SupplierOrder>>()
                     .To<SupplierOrderRepository>()
                     .AsTransient();

        selfContainer.Add<SupplierOrder>()
                     .AsScoped<SupplierOrderFormWindow>();
        
        selfContainer.Add<Entities.Form>()
                     .AsScoped<SupplierOrderFormWindow>()
                     .EnforceInstantiateOnBegin();
        
        selfContainer.Add<EditFormState>()
                     .AsScoped<SupplierOrderFormWindow>();
        
        selfContainer.Add<CreateFormState>()
                     .AsScoped<SupplierOrderFormWindow>();
        
        
        selfContainer.Add<SaveButVm>()
                     .AsScoped<SupplierOrderFormWindow>();
        
        selfContainer.Add<SaveAction>()
                     .AsScoped<SupplierOrderFormWindow>()
                     .EnforceInstantiateOnBegin();

        selfContainer.Add<CounterpartySelectButVm>()
                     .AsScoped<SupplierOrderFormWindow>();

        selfContainer.Add<CounterpartySelectAction>()
                     .AsScoped<SupplierOrderFormWindow>()
                     .EnforceInstantiateOnBegin();

        selfContainer.Add<SupplierOrderFormWindow>()
                     .AsTransient();
        
        return selfContainer;
    }
}