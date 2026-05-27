using Comp_v4._Installers;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Actions;

public class ActionAnalogsSave : BaseActionAsyncCompletion, IRuntimeParamsContainer<Analog>
{
    protected Analog _analog;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    public ActionAnalogsSave(SaveAnalogButVm but, IWindowOrderLocator windowOrderLocator) : base(but) {
        _windowOrderLocator = windowOrderLocator;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var tasks = new List<Task>();

        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IAnalogSaveHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.Save(subscriberTcs, RuntimeParam);
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });

        await Task.WhenAll(tasks);
    }

    public override bool CanPerform() {
        return RuntimeParam is { RelatedComponent: not null };
    }
    
    public Analog RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<Analog>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _analog;
        }
        set {
            _analog = value;
        }
    }
}