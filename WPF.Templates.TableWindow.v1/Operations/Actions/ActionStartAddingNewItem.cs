using System.Windows;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Entities.Cells.States;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Operations.Commands;
using WPF.Templates.TableWindow.v1.Operations.Commands.Ui;
using WPF.Templates.TableWindow.v1.Operations.Transactions;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

public class ActionStartAddingNewItem<TWindow, T> : BaseAction<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity, new()
{
    protected readonly Cell<TWindow, T> _cell;
    
    public ActionStartAddingNewItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, Cell<TWindow, T> cell) 
        : base(scheduler, context, commandFactory) {
        _cell = cell;
    }

    public override async Task<BaseAction<TWindow, T>> PerformAsync(object? parameter = null) {
        _scheduler.BeginTransaction<TransactionAddItem>();
        
        var createRawCommand = new CreateRawCommand<TWindow, T>(_context);
        
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringHandler>(h => h?.OnSourceCollectionStartEditing());

        await _scheduler.RegisterCommandInto<TransactionAddItem>(
                             new CellChangeStateCommand<TWindow, T>(_context, _cell, _cell.GetState<CellStateAddItem<TWindow, T>>())
                             )
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionAddItem>(
                             _commandFactory.CreateCommand<RememberSelectionCommand<TWindow, T>, object>(_context)
                             )
                        .ExecuteLastRegisteredAsync();

        await _scheduler.RegisterCommandInto<TransactionAddItem>(createRawCommand)
                        .ExecuteLastRegisteredAsync();

        var focusCommand = _commandFactory.CreateCommand<FocusCellCommand<TWindow, T>, T>(createRawCommand.Item);
        await _scheduler.RegisterCommandInto<TransactionAddItem>(focusCommand)
                        .ExecuteLastRegisteredAsync();
        
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringHandler>(h => h?.OnSourceCollectionStopEditing());

        _scheduler.CommitTransaction<TransactionAddItem>();
        
        return this;
    }

    public override bool CanPerform() {
        var cellCurrentState = _cell.CurrentState;
        return cellCurrentState is CellStateIdle<TWindow, T>;
    }

    public override async Task CancelAsync(object? parameter = null) {
        await _scheduler.CommitTransaction<TransactionAddItem>().UndoAsync();
    }
}

