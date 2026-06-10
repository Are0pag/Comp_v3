using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Utils.EventBus;
using Utils.WPF.Windows;

namespace Comp_v4.TableWindows.Analogs;

public partial class AnalogsTableWindow : TableWindowBase, IDisposable, IRuntimeParamsResolver<AnalogsTableWindow>
{
    protected readonly AddAnalogButtonVm _addAnalogButtonVm;
    protected readonly EditAnalogButVm _editAnalogButVm;
    protected readonly DeleteAnalogButVm _deleteAnalogButVm;
    public AnalogsTableWindow(AnalogsTableVm analogsTableVm, AddAnalogButtonVm addAnalogButtonVm, EditAnalogButVm editAnalogButVm, DeleteAnalogButVm deleteAnalogButVm) {
        InitializeComponent();
        
        _addAnalogButtonVm = addAnalogButtonVm;
        _editAnalogButVm = editAnalogButVm;
        _deleteAnalogButVm = deleteAnalogButVm;

        MainDataGrid.DataContext = analogsTableVm;
        
        AddAnalogButton.DataContext = addAnalogButtonVm;
        EditButton.DataContext = editAnalogButVm;
        DeleteBut.DataContext = _deleteAnalogButVm;
        
        InfoDataGrid_ContextMenu_AddNewItemCommand.DataContext = _addAnalogButtonVm;
        InfoDataGrid_ContextMenu_EditItemCommand.DataContext = _editAnalogButVm;
        InfoDataGrid_ContextMenu_DeleteItemCommand.DataContext = _deleteAnalogButVm;
        
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<AnalogsTableWindow> container) {
        container.RuntimeParam = this;
    }


    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }

    private async void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        await _editAnalogButVm.OnClickAsync(); 
    }

    private void MainDataGrid_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Insert:
                if (_addAnalogButtonVm.CanClick())
                    _addAnalogButtonVm.OnClickAsync();
                break;
            
            case Key.Delete:
                if (_deleteAnalogButVm.CanClick())
                    _deleteAnalogButVm.OnClickAsync();
                break;
        }
    }

    private void MainDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
        _addAnalogButtonVm.NotifyCanExecute();
        _editAnalogButVm.NotifyCanExecute();
        _deleteAnalogButVm.NotifyCanExecute();
    }
}