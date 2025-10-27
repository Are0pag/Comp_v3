using Comp_v4.Entry.Vm.Buts;
using Comp_v4.NomDict.View;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Actions;

public class OpenNomDictAction : BaseActionAsyncCompletion
{
    protected readonly IServiceScopeFactory _openNomDictHandler;
    protected TaskCompletionSource? _currentTcs;
    public OpenNomDictAction(NomDictButVm button, IServiceScopeFactory openNomDictHandler) : base(button) {
        _openNomDictHandler = openNomDictHandler;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _currentTcs = tcs;
        using (var scope = _openNomDictHandler.CreateScope()) {
            var window = scope.ServiceProvider.GetRequiredService<NomDictWindow>();
            window.Closed += (sender, args) => {
                tcs.SetResult();
            };
            window.Show();
            await tcs.Task;
        }
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}