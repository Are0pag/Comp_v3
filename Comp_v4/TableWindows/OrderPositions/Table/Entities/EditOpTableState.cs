using Comp_v4.TableWindows.OrderPositions.Form;
using Comp_v4.TableWindows.OrderPositions.Form.Actions;
using Comp_v4.TableWindows.OrderPositions.Form.Entities;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.OrderPositions.Table.Entities;

public class EditOpTableState : BaseOpState
{
    protected readonly IServiceProvider _serviceProvider;

    public EditOpTableState(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Create(TaskCompletionSource tcs, OpTable opTable, object? o) {
        throw new NotImplementedException();
        /*if (o is not SupplierOrder so)
            throw new InvalidOperationException();

        var window = ActivatorUtilities.CreateInstance<OrderPositionForm>(_serviceProvider, new OrderPosition() {
            SupplierOrder = so
        });

        var parent = new InstanceContainer<OrderPositionsTableWindow>().RuntimeParam;
        window.Owner = parent;
        WindowService.BindChildToParent(parent, window);

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        ResolveRelated();

        var table = _serviceProvider.GetRequiredService<OpForm>();
        await table.ChangeState(table.GetState<CreateOpFormState>(), table);

        window.Show();
        await tcs.Task;*/
    }

    public override async Task Edit(TaskCompletionSource tcs, OpTable opTable, OrderPosition op, object? o) {
        var window = ActivatorUtilities.CreateInstance<OrderPositionForm>(_serviceProvider, op);
        var parent = new InstanceContainer<OrderPositionsTableWindow>().RuntimeParam;
        window.Owner = parent;
        WindowService.BindChildToParent(parent, window);
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        ResolveRelated();
        
        var table = _serviceProvider.GetRequiredService<OpForm>();
        await table.ChangeState(table.GetState<EditOpFormState>(), table);
        
        window.Show();
        await tcs.Task;
    }

    private void ResolveRelated() {
        _serviceProvider.GetRequiredService<SaveOrderPositionAction>();
    }
}