using System.Windows;
using System.Windows.Controls;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Events.Requests;
using WPF.Templates.TableWindow.v1.Vm;

namespace WPF.Templates.TableWindow.v1.Entities;

public class ModuleContext<TWindow, T> : IDataGridRequester<TWindow>
    where TWindow : Window
    where T : class
{
    protected DataGrid? _dataGrid;
    public ModuleContext(DataGridViewModel<T> dataGridViewModel) {
        DataGridViewModel = dataGridViewModel;
    }

    public required DataGridViewModel<T> DataGridViewModel { get; init; }

    public DataGrid DataGrid {
        get {
            if (_dataGrid == null) 
                EventBus<IGlobSubscriber>.RaiseEvent<IDataGridRequestResolver<TWindow>>(h => h.GetGrid(this));
            return _dataGrid!;
        }
        set => _dataGrid = value;
    }
}