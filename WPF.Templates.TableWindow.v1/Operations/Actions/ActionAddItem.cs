using System.Windows;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Operations.Commands.Db;
using WPF.Templates.TableWindow.v1.Operations.Transactions;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

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