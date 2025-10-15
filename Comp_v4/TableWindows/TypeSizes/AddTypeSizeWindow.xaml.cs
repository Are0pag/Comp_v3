using System.Windows;
using System.Windows.Input;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.TypeSizes;

public partial class AddTypeSizeWindow : Window, IDisposable, ITypeSizeCreateHandler
{
    public AddTypeSizeWindow(ImageFieldVmBase imageFieldVm, TypeSize typeSize, ButtonSaveNewItemForm buttonSaveNewItemForm) {
        InitializeComponent();
        DataContext = typeSize;
        ImageFieldControl.DataContext = imageFieldVm;
        ButtonSave.DataContext = buttonSaveNewItemForm;
        EventBus<ITypeSizesWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ITypeSizesWindowSubscriber>.Unsubscribe(this);
    }

    public async Task OnCreate(object? parameter = null) {
        Close();
    }

    private void AddTypeSizeWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
    }
}