using System.Windows;
using System.Windows.Controls;
using Comp_v4;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.Events.Requests;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates;

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