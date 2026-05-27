using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Events;
using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.NomDict.View;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions.Images;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.NomDict.Entities;

public class EditGridState : BaseSGridState, IRuntimeParamsContainer<NomDictWindow>
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly DataGridVm _dataGridVm;
    protected readonly TreeViewVm _treeViewVm;
    protected NomDictWindow _item;


    public EditGridState(IServiceProvider serviceProvider, DataGridVm dataGridVm, TreeViewVm treeViewVm) {
        _serviceProvider = serviceProvider;
        _dataGridVm = dataGridVm;
        _treeViewVm = treeViewVm;
    }

    public override async Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs mouseButtonEventArgs, Grid grid) {
        await EditComp(tcs, null, grid);
    }

    public override async Task Add(TaskCompletionSource tcs, object? parameter, Grid grid) {
        var component = new Component() {
            Category = _treeViewVm.SelectedCategory!
        };
        var window = ActivatorUtilities.CreateInstance<CompCardWindow>(_serviceProvider, component);
        window.Owner = RuntimeParam;
        ResolveRelated();
        
        var card = _serviceProvider.GetRequiredService<CardComp>();
        await card.ChangeState(card.GetState<CreateStateCardComp>(), card);
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        
        _serviceProvider.GetRequiredService<IWindowOrderLocator>().RegisterWindow(window);
        
        EventBus<ICompCardSubscriber>.RaiseEvent<ICompCardLoadedHandler>(h => h?.OnCompCardLoaded(component));
        WindowService.BindChildToParent(RuntimeParam, window);
        window.Show();
        await tcs.Task;
    }

    public override async Task EditComp(TaskCompletionSource tcs, object? parameter, Grid grid) {
        if (_dataGridVm.SelectedItem is not { } component)
            // Fixed: Added meaningful exception message
            throw new InvalidOperationException("No component selected for editing");
        var window = ActivatorUtilities.CreateInstance<CompCardWindow>(_serviceProvider, component);
        window.Owner = RuntimeParam;
        ResolveRelated();
        
        var card = _serviceProvider.GetRequiredService<CardComp>();
        await card.ChangeState(card.GetState<EditStateCardComp>(), card);
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
            _serviceProvider.GetRequiredService<IWindowOrderLocator>().UnregisterWindow(window);
        };
        _serviceProvider.GetRequiredService<IWindowOrderLocator>().RegisterWindow(window);
        EventBus<ICompCardSubscriber>.RaiseEvent<ICompCardLoadedHandler>(h => h?.OnCompCardLoaded(component));
        
        WindowService.BindChildToParent(RuntimeParam, window);
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

        _serviceProvider.GetRequiredService<OpenAnalogTableAction>();
    }
    
    public NomDictWindow RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<NomDictWindow>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _item;
        }
        set => _item = value;
    }
}