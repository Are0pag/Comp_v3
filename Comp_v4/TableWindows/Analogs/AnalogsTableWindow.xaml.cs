using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Utils.EventBus;
using Utils.WPF.Windows;

namespace Comp_v4.TableWindows.Analogs;

public partial class AnalogsTableWindow : TableWindowBase, IDisposable, IRuntimeParamsResolver<AnalogsTableWindow>
{
    protected readonly EditAnalogButVm _editAnalogButVm;
    public AnalogsTableWindow(AnalogsTableVm analogsTableVm, AddAnalogButtonVm addAnalogButtonVm, EditAnalogButVm editAnalogButVm) {
        InitializeComponent();
        _editAnalogButVm = editAnalogButVm;
        MainDataGrid.DataContext = analogsTableVm;
        AddAnalogButton.DataContext = addAnalogButtonVm;
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<AnalogsTableWindow> container) {
        container.RuntimeParam = this;
    }


    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }

    private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        //EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IMouseDoubleClickHandler>(h => h.OnMouseDoubleClick(sender, e));
        //_editAnalogButVm.OnClickAsync(); Не делаю, нахуй вообще нужно
    }
}