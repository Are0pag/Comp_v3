using System.Windows.Input;
using Comp_v4.TableWindows.Analogs._Installers;
using Comp_v4.TableWindows.Analogs.Actions;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class EditAnalogsTableState : BaseAnalogsTableState
{
    protected readonly AnalogsTableVm _analogsTableVm;
    protected readonly IServiceProvider _serviceProvider;

    public EditAnalogsTableState(AnalogsTableVm analogsTableVm, IServiceProvider serviceProvider) {
        _analogsTableVm = analogsTableVm;
        _serviceProvider = serviceProvider;
    }

    public override async Task OnMouseDoubleClick(TaskCompletionSource tcs, AnalogsTable table, object sender, MouseButtonEventArgs e) {
        await Edit(tcs, table);
    }

    public override async Task Add(TaskCompletionSource tcs, AnalogsTable analogsTable) {
        var analog = new Analog();
        var window = ActivatorUtilities.CreateInstance<AnalogsFormWindow>(_serviceProvider, analog);

        _serviceProvider.GetRequiredService<ActionAnalogsSave>();
        
        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        
        window.Show();
        await tcs.Task;
    }

    public override async Task Edit(TaskCompletionSource tcs, AnalogsTable analogsTable) {
        if (_analogsTableVm.SelectedItem is not { } analog) 
            throw new InvalidOperationException();
        var window = ActivatorUtilities.CreateInstance<AnalogsFormWindow>(_serviceProvider, analog);
        
        var form = _serviceProvider.GetRequiredService<AnalogsForm>();
        await form.ChangeState(form.GetState<EditAnalogsFormState>(), form);

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        
        window.Show();
        await tcs.Task;
    }
}