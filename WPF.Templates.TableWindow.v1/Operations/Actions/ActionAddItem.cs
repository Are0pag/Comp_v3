using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;

namespace WPF.Templates;

public class ActionAddItem<TWindow, T>  : ActionUpdateItem<TWindow, T> 
    where TWindow : Window
    where T : class, IDbEntity
{
    public ActionAddItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T>  context, ICommandFactory commandFactory) 
        : base(scheduler, context, commandFactory) {
    }

    protected override async Task InitTransaction(Args args) {
        await _scheduler.RegisterCommandInto<TrEditCell>(args.RememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        
        _scheduler.RegisterCommandInto<TrEditCell>(_commandFactory.CreateCommand<UpdateItemCommand<T>, T>(args.Item));
    }
}