using Comp_v4.TableWindows.OrderPositions.Form;
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
        var window = ActivatorUtilities.CreateInstance<OrderPositionForm>(_serviceProvider, new OrderPosition());
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
        await tcs.Task;
    }

    public override async Task Edit(TaskCompletionSource tcs, OpTable opTable, OrderPosition op, object? o) {
        throw new NotImplementedException();
    }
}