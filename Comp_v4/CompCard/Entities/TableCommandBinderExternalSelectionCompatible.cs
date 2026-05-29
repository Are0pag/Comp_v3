using System.Windows;
using System.Windows.Input;
using Comp_v4.CompCard.Events;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.CompCard.Entities;

public class TableCommandBinderExternalSelectionCompatible<TWindow, T> : TableCommandBinderFilteringCompatible<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity, new()
{
    protected readonly ModuleContext<TWindow, T> _context;
    
    public TableCommandBinderExternalSelectionCompatible(ActionStartAddingNewItem<TWindow, T> actionStartAddingNewItem, 
                                                         ActionDeleteItem<TWindow, T> actionDeleteItem, 
                                                         ModuleContext<TWindow, T> context) 
        : base(actionStartAddingNewItem, actionDeleteItem) {
        _context = context;
    }

    public override async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        if (!((Window)sender).IsLoaded) 
            return;
        
        // у каждого окна свой тип обработчика, а ивент общий, поэтому необходимо убедиться что обработчик соотв. типу окна
        if (_context.GetType().GetGenericArguments()[0] != sender.GetType())
            return;
        
        await base.OnPreviewKeyDown(sender, e);
        switch (e.Key) {
            case Key.Enter when Keyboard.Modifiers == ModifierKeys.Shift:
                EventBus<ICompCardSubscriber>
                   .RaiseEvent<IExternalTableInputHandler<T>>(h => {
                        h?.HandleTableInput(_context.DataGridViewModel.SelectedItem);
                    });
                EventBus<ICompCardSubscriber>.RaiseEvent<ITableWindowHandler>(h => h?.HandleClosingTableWindow<TWindow>(null));
                break;
        }
    }
}