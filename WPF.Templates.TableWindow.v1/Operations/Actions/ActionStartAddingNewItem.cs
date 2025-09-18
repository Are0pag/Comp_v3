using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class ActionStartAddingNewItem : BaseAction
{
    protected readonly Cell _cell;
    
    public ActionStartAddingNewItem(IDataGridCommandScheduler scheduler, ModuleContext context, CommandFactory commandFactory, Cell cell) : base(scheduler, context, commandFactory) {
        _cell = cell;
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        _scheduler.BeginTransaction<TransactionAddItem>();
        
        var createRawCommand = new CreateRawCommand(_context);
        
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringHandler>(h => h?.OnSourceCollectionStartEditing());

        await _scheduler.RegisterCommandInto<TransactionAddItem>(new CellChangeStateCommand(_context, _cell, _cell.GetState<CellStateAddItem>()))
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionAddItem>(_commandFactory.CreateCommand<RememberSelectionCommand, object>(_context))
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<TransactionAddItem>(createRawCommand)
                        .ExecuteLastRegisteredAsync();

        var focusCommand = _commandFactory.CreateCommand<FocusCellCommand, ConditionalDesignation>(createRawCommand.Item);
        await _scheduler.RegisterCommandInto<TransactionAddItem>(focusCommand)
                        .ExecuteLastRegisteredAsync();
        
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringHandler>(h => h?.OnSourceCollectionStopEditing());

        _scheduler.CommitTransaction<TransactionAddItem>();
        
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle;
    }

    public override async Task CancelAsync(object? parameter = null) {
        await _scheduler.CommitTransaction<TransactionAddItem>().UndoAsync();
    }
}

