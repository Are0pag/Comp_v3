using Comp_v4.TableWindows.OrderPositions.Form;
using Comp_v4.TableWindows.OrderPositions.Form.Actions;
using Comp_v4.TableWindows.OrderPositions.Form.Entities;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF;

namespace Comp_v4.TableWindows.OrderPositions.Table.Entities;

public class EditOpTableState : BaseOpState
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    
    public EditOpTableState(IServiceProvider serviceProvider, IWindowOrderLocator windowOrderLocator) {
        _serviceProvider = serviceProvider;
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task Create(TaskCompletionSource tcs, OpTable opTable, object? o) {
        if (o is not SupplierOrder so)
            throw new InvalidOperationException();
        
        var window = ActivatorUtilities.CreateInstance<OrderPositionForm>(_serviceProvider, new OrderPosition() {
            RelatedSupplierOrder = so
        });

        var parent = new InstanceContainer<OrderPositionsTableWindow>().RuntimeParam;
        window.Owner = parent;
        WindowService.BindChildToParent(parent, window);
        
        //_windowOrderLocator.RegisterWindow(window);
        window.Closed += (sender, args) => {
            _windowOrderLocator.UnregisterWindow(window);
            tcs.TrySetResult();
        };
        ResolveRelated();

        var table = _serviceProvider.GetRequiredService<OpForm>();
        await table.ChangeState(table.GetState<CreateOpFormState>(), table);
        
        window.Show();
        await tcs.Task;
    }

    public override async Task Edit(TaskCompletionSource tcs, OpTable opTable, OrderPosition op, object? o) {
        var window = ActivatorUtilities.CreateInstance<OrderPositionForm>(_serviceProvider, op);
        //_windowOrderLocator.RegisterWindow(window);
        var parent = new InstanceContainer<OrderPositionsTableWindow>().RuntimeParam;
        window.Owner = parent;
        WindowService.BindChildToParent(parent, window);
        
        window.Closed += (sender, args) => {
            _windowOrderLocator.UnregisterWindow(window);
            tcs.TrySetResult();
        };
        ResolveRelated();
        
        var table = _serviceProvider.GetRequiredService<OpForm>();
        await table.ChangeState(table.GetState<EditOpFormState>(), table);
        
        window.Show();
        await tcs.Task;
    }

    private void ResolveRelated() {
        _serviceProvider.GetRequiredService<SelectPositionAction>();
        _serviceProvider.GetRequiredService<SaveOrderPositionAction>();
    }
}