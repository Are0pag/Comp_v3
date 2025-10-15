using System.Windows;
using Comp.Db.Contracts;
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
    public ActionAddItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, IRepository<T> repository) : base(scheduler, context, commandFactory, repository) {
    }

    protected override async Task InitTransaction(Args args) {
        await _scheduler.RegisterCommandInto<TrEditCell>(args.RememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        
        _scheduler.RegisterCommandInto<TrEditCell>(new AddItemCommand<T>(args.Item, _repository));
    }
}