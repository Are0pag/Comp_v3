using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.Contracts;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Dialogs;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Entities.InputHandlers;

public class TableCommandBinder<TWindow, T> : IPreviewKeyDownHandler
    where TWindow : Window
    where T : class, IDbEntity, new()
{
    protected readonly ActionStartAddingNewItem<TWindow, T> _actionStartAddingNewItem;
    protected readonly ActionDeleteItem<TWindow, T> _actionDeleteItem;
    protected readonly ActionCancel<TWindow, T> _actionCancel;
    
    public TableCommandBinder(ActionStartAddingNewItem<TWindow, T> actionStartAddingNewItem, ActionDeleteItem<TWindow, T> actionDeleteItem, ActionCancel<TWindow, T> actionCancel) {
        _actionStartAddingNewItem = actionStartAddingNewItem;
        _actionDeleteItem = actionDeleteItem;
        _actionCancel = actionCancel;
        EventBus<IGlobSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public virtual async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            //case Key.Add when _actionStartAddingNewItem.CanPerform():
            case Key.Insert /*Key.OemPlus*/ when _actionStartAddingNewItem.CanPerform():
                await _actionStartAddingNewItem.PerformAsync();
                break;
            
            case Key.Delete when _actionDeleteItem.CanPerform():
                if (!await _actionDeleteItem.TryExecuteAsync<Component>()) {
                    MessageBox.Show("Невозможно удалить элемент, так как он ещё используется другим компонентом");
                    return;
                }
                
                var isConfirmed = DialogService.ShowConfirmation("Удаление", $"Вы уверены что хотите удалить элемент?");
                if (!isConfirmed) return;
                
                await _actionDeleteItem.PerformAsync();
                break;
            
            case Key.F1:
                var cb = VisualHelper.FindVisualChild<CheckBox>((Window)sender);
                cb.IsChecked = !cb.IsChecked;
                break;
            
            case Key.Escape:
                try {
                    if (_actionCancel.CanPerform()) {
                        await _actionCancel.PerformAsync();
                    }
                    ((Window)sender).Close();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                break;
        }
    }

    public Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        return Task.CompletedTask;
    }
}