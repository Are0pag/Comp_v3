using System.Windows;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Entities.Cells.States;

public class CellStateUpdate<TWindow, T>  : BaseCellStateInput<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    public CellStateUpdate(IDataGridCommandScheduler scheduler, 
                           ModuleContext<TWindow, T>  context,  
                           ICommandFactory factory, 
                           ActionUpdateItem<TWindow, T> actionUpdateItem, 
                           ValidatorBase<T> validator) 
        : base(scheduler, context, factory, validator) {
        _action = actionUpdateItem;
    }
}