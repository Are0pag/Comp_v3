using System.Windows.Input;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.NomDict.Entities;

public class EditGridState : BaseSGridState
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly DataGridVm _dataGridVm;

    public EditGridState(IServiceProvider serviceProvider, DataGridVm dataGridVm) {
        _serviceProvider = serviceProvider;
        _dataGridVm = dataGridVm;
    }

    public override async Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs mouseButtonEventArgs, Grid grid) {
        if (_dataGridVm.SelectedItem is not { } component)
            throw new Exception();
        var window = ActivatorUtilities.CreateInstance<CompCardWindow>(_serviceProvider, component);
        ResolveRelated();
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
        await tcs.Task;
    }

    public override async Task Add(TaskCompletionSource tcs, object? parameter, Grid grid) {
        var window = ActivatorUtilities.CreateInstance<CompCardWindow>(_serviceProvider, new Component());
        ResolveRelated();
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
        await tcs.Task;
    }

    private void ResolveRelated() {
        _serviceProvider.GetRequiredService<SetUrlAction>();
        _serviceProvider.GetRequiredService<SetUrlAlternativeAction>();
        _serviceProvider.GetRequiredService<SetFilePathAction>();
        
        _serviceProvider.GetRequiredService<SelectImageAction>();
        _serviceProvider.GetRequiredService<OpenImageAction>();
        _serviceProvider.GetRequiredService<ClearImageAction>();
    }
}