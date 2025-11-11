using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.Entry.Vm.Buts;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Operations.Actions.Components;
using Comp_v4.NomDict.View;
using DI;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenNomDictAction : BaseActionAsyncCompletion
{
    protected readonly IServiceProvider _openNomDictHandler;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    
    protected TaskCompletionSource? _currentTcs;
    public OpenNomDictAction(NomDictButVm button, IServiceProvider openNomDictHandler, IWindowOrderLocator windowOrderLocator) : base(button) {
        _openNomDictHandler = openNomDictHandler;
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var window = _openNomDictHandler.GetRequiredService<NomDictWindow>();
        
        _openNomDictHandler.GetRequiredService<AddCategoryAction>();
        _openNomDictHandler.GetRequiredService<DeleteCategoryAction>();
        _openNomDictHandler.GetRequiredService<UpdateCategoryNameAction>();
        
        _openNomDictHandler.GetRequiredService<AddComponentAction>();
        _openNomDictHandler.GetRequiredService<EditComponentAction>();
        
        _openNomDictHandler.GetRequiredService<SaveComponentAction>();
        
        _windowOrderLocator.RegisterWindow(window);
        
        window.Show();
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        await tcs.Task;
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}