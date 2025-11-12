using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs._Installers;
using Comp_v4.TableWindows.Analogs.Actions;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class EditAnalogsTableState : BaseAnalogsTableState, IRuntimeParamsContainer<Component>
{
    protected readonly AnalogsTableVm _analogsTableVm;
    protected readonly IServiceProvider _serviceProvider;
    protected Component _component;

    public EditAnalogsTableState(AnalogsTableVm analogsTableVm, IServiceProvider serviceProvider) {
        _analogsTableVm = analogsTableVm;
        _serviceProvider = serviceProvider;
    }

    public override async Task OnMouseDoubleClick(TaskCompletionSource tcs, AnalogsTable table, object sender, MouseButtonEventArgs e) {
        await Edit(tcs, table);
    }

    public override async Task Add(TaskCompletionSource tcs, AnalogsTable analogsTable) {
        var analog = new Analog() {
            SourceComponent = RuntimeParam
        };
        var window = ActivatorUtilities.CreateInstance<AnalogsFormWindow>(_serviceProvider, analog);

        ResolveRelated();

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
        
        ResolveRelated();

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        
        window.Show();
        await tcs.Task;
    }

    private void ResolveRelated() {
        _serviceProvider.GetRequiredService<ActionAnalogsSave>();
        _serviceProvider.GetRequiredService<AnalogsForm>();
    }

    public Component RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<Component>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _component;
        }
        set {
            _component = value;
        }
    }
}