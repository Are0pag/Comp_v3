using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.TableWindows.TypeSizes;

public class ActionAddingNewTs : ActionStartAddingNewItem<TypeSizesTableWindow, TypeSize>
{
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource<BaseAction<TypeSizesTableWindow, TypeSize>>? _tcs;
    public ActionAddingNewTs(IDataGridCommandScheduler scheduler, 
                               ModuleContext<TypeSizesTableWindow, TypeSize> context, 
                               ICommandFactory commandFactory, 
                               Cell<TypeSizesTableWindow, TypeSize> cell, 
                               IServiceProvider serviceProvider) 
        : base(scheduler, context, commandFactory, cell) {
        _serviceProvider = serviceProvider;
        Console.WriteLine("ActionAddingNewTs");
    }

    public override async Task<BaseAction<TypeSizesTableWindow, TypeSize>> PerformAsync(object? parameter = null) {
        _tcs = new TaskCompletionSource<BaseAction<TypeSizesTableWindow, TypeSize>>();
        var window = ActivatorUtilities.CreateInstance<AddTypeSizeWindow>(_serviceProvider, new TypeSize());
        window.Closed += (sender, args) => {
            _tcs.SetResult(this);
        };
        window.Show();
        
        var res = await _tcs.Task;
        _tcs = null;
        return res;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _tcs == null;
    }
}