using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Operations.Actions.Components;

public class AddComponentAction : BaseAsyncActionButtonInvoked
{
    protected readonly Action<object?> _openCompWindow;
    
    public AddComponentAction(BaseAsyncBButtonVm buttonVm, Action<object?> openCompWindow) : base(buttonVm) {
        _openCompWindow = openCompWindow;
    }

    public override async Task PerformAsync(object? parameter) {
        throw new NotImplementedException();
    }

    public override bool CanPerform() {
        throw new NotImplementedException();
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}