using System.Windows.Controls;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates;

public abstract class ModuleContext
{
    public ModuleContext(ViewModel viewModel) {
        ViewModel = viewModel;
    }

    public required ViewModel ViewModel { get; init; }

    public abstract DataGrid? DataGrid { get; protected set; } /* в геттере запрос через IDataGridRequester */
}