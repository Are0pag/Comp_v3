using CommunityToolkit.Mvvm.Input;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Infrastructure.Command.Base;
using Infrastructure.Command.Heterochromic;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public partial class SaveChangesButVm : BaseButtonsVm
{
    protected readonly HeterochromicCommandScheduler _scheduler;
    public SaveChangesButVm(CognDesignGridVm condDesignGridVm, HeterochromicCommandScheduler scheduler) : base(condDesignGridVm) {
        _scheduler = scheduler;
        _scheduler.OnCommandExecuted += HandleCommandExecuted;
    }

    public override void Dispose() => _scheduler.OnCommandExecuted -= HandleCommandExecuted;
    public override void NotifyCanExecute() {
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    protected async Task SaveChangesAsync() {
        await _condDesignGridVm.StateProvider.CurrentState.SaveChanges();
    }

    protected bool CanSaveChanges() {
        return _condDesignGridVm.StateProvider.CurrentState.CanSaveChanges();
    }

    protected void HandleCommandExecuted(CommandScheduler<IDeferredCommand>.CommandAction action, IDeferredCommand command) {
        SaveChangesCommand.NotifyCanExecuteChanged();
    }
}