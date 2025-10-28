using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.SupplierOrders.Installers;

public interface ISupplierOrderFormWindowFactory
{
    SupplierOrderFormWindow Create(IServiceProvider sp, SupplierOrder order);
}


public class SupplierOrderFormWindowFactory : ISupplierOrderFormWindowFactory
{
    public SupplierOrderFormWindow Create(IServiceProvider sp, SupplierOrder order) {
        return ActivatorUtilities
           .CreateInstance<
                SupplierOrderFormWindow
            >(sp,
              order,
              ActivatorUtilities.CreateInstance<OrderStatusEnumsVm>(sp, order),
              ActivatorUtilities.CreateInstance<VatStatusEnumVm>(sp, order)
             );
    }
}

public interface ISoFormFactory
{
    SoForm Create<TInitialState>() where TInitialState : BaseSoFormState;
}

public class SoFormFactory : ISoFormFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SoFormFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public SoForm Create<TInitialState>() where TInitialState : BaseSoFormState {
        var initialState = _serviceProvider.GetService<TInitialState>();
        var states =  _serviceProvider.GetServices<BaseSoFormState>().ToList();
        return ActivatorUtilities.CreateInstance<SoForm>(_serviceProvider, states, initialState!);
    }
}