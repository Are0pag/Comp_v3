using System.Windows.Input;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Events;
using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions.Images;
using Utils.EventBus;

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
        await EditComp(tcs, null, grid);
    }

    public override async Task Add(TaskCompletionSource tcs, object? parameter, Grid grid) {
        var component = new Component();
        var window = ActivatorUtilities.CreateInstance<CompCardWindow>(_serviceProvider, component);
        ResolveRelated();
        
        var card = _serviceProvider.GetRequiredService<CardComp>();
        await card.ChangeState(card.GetState<CreateStateCardComp>(), card);
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        EventBus<ICompCardSubscriber>.RaiseEvent<ICompCardLoadedHandler>(h => h?.OnCompCardLoaded(component));
        window.Show();
        await tcs.Task;
    }

    public override async Task EditComp(TaskCompletionSource tcs, object? parameter, Grid grid) {
        if (_dataGridVm.SelectedItem is not { } component)
            throw new Exception();
        var window = ActivatorUtilities.CreateInstance<CompCardWindow>(_serviceProvider, component);
        ResolveRelated();
        
        var card = _serviceProvider.GetRequiredService<CardComp>();
        await card.ChangeState(card.GetState<EditStateCardComp>(), card);
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        EventBus<ICompCardSubscriber>.RaiseEvent<ICompCardLoadedHandler>(h => h?.OnCompCardLoaded(component));
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