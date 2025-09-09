using System.Windows.Controls;
using Comp_v4;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.Events.Requests;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates;

public class ModuleContext : IDataGridRequester<TargetWindow>
{
    protected DataGrid? _dataGrid;
    public ModuleContext(DataGridViewModel dataGridViewModel) {
        DataGridViewModel = dataGridViewModel;
    }

    public required DataGridViewModel DataGridViewModel { get; init; }

    public DataGrid DataGrid {
        get {
            if (_dataGrid == null) 
                EventBus<IGlobSubscriber>.RaiseEvent<IDataGridRequestResolver<TargetWindow>>(h => h.GetGrid(this));
            return _dataGrid!;
        }
        set => _dataGrid = value;
    }
}