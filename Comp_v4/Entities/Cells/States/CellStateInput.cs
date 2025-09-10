using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;

namespace WPF.Templates.TableWindow.States;

public class CellStateInput : BaseCellState
{
    public CellStateInput(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public override async Task  OnBeginning(object? sender, DataGridBeginningEditEventArgs e) {
        base.OnBeginning(sender, e);
    }

    public override async Task  OnEnding(object? sender, DataGridCellEditEndingEventArgs e) {
    }

    public override async Task  OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
                await _scheduler.BeginTransaction<TransactionUpdateItem>()
                                .RegisterCommand(new RememberInputTextCommand(_context))
                                .ExecuteLastRegisteredAsync();
                
                await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new UpdateItemCommand(_context))
                                .ExecuteLastRegisteredAsync();

                _scheduler.CommitTransaction<TransactionUpdateItem>();
            break;
            
            case Key.Tab:
                
                break;
        }
    }
}