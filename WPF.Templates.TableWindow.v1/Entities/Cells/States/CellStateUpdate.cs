using System.ComponentModel.DataAnnotations;
using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Infrastructure.Command;
using WPF.Services.Validation;

namespace WPF.Templates.TableWindow.States;

public class CellStateUpdate<TWindow, T>  : BaseCellStateInput<TWindow, T>
    where TWindow : Window
    where T : class
{
    public CellStateUpdate(IDataGridCommandScheduler scheduler, 
                           ModuleContext<TWindow, T>  context,  
                           ICommandFactory factory, 
                           ActionUpdateItem actionUpdateItem, 
                           ValidatorBase<T> validator) 
        : base(scheduler, context, factory, validator) {
        _action = actionUpdateItem;
    }
}