using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Events.Update;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

public abstract class BaseButtonsVm<TWindow, T, TContext> : ObservableObject, INotifyConditionalsChanged
    where TWindow : Window
    where T : class, IDbEntity
    where TContext : BaseAction<TWindow, T>
{
    protected readonly TContext _context;
    
    public BaseButtonsVm(TContext context) {
        _context = context;
        EventBus<WPF.Templates.TableWindow.v1.Events.Update.IGlobalButtonEvent>.Subscribe(this);
    }
    
    public virtual void Dispose() {
        EventBus<WPF.Templates.TableWindow.v1.Events.Update.IGlobalButtonEvent>.Unsubscribe(this);
    }

    public abstract void NotifyCanExecute();
}