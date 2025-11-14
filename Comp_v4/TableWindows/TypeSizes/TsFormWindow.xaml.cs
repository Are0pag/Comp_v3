using System.Windows;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.TypeSizes;

public partial class TsFormWindow : Window, IDisposable, ITypeSizeCreateHandler, IRuntimeParamsResolver<TypeSize>
{
    protected TypeSize _typeSize;
    public TsFormWindow(TsImageFieldVm imageFieldVm, TypeSize typeSize, ButtonSaveNewItemForm buttonSaveNewItemForm) {
        InitializeComponent();
        _typeSize = typeSize;
        DataContext = typeSize;
        ImageFieldControl.DataContext = imageFieldVm;
        ButtonSave.DataContext = buttonSaveNewItemForm;
        EventBus<ITypeSizesWindowSubscriber>.Subscribe(this);
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<TypeSize> container) {
        container.RuntimeParam = _typeSize;
    }

    public void Dispose() {
        EventBus<ITypeSizesWindowSubscriber>.Unsubscribe(this);
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }

    public async Task OnCreate(object? parameter = null) {
        Close();
    }

    private void AddTypeSizeWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
    }
}