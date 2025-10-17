using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Actions;

public class ActionSave : BaseActionAsyncCompletion
{
    protected readonly Analog _analog;
    
    public ActionSave(SaveButVm but, Analog analog) : base(but) {
        _analog = analog;
    }


    public override async Task Perform(TaskCompletionSource tcs) {
        var tasks = new List<Task>();

        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<ISaveHandler>(h => {
            var subscriberTcs = new TaskCompletionSource();
            tasks.Add(subscriberTcs.Task);

            try {
                h?.Save(subscriberTcs);
            }
            catch (Exception ex) {
                subscriberTcs.TrySetException(ex);
            }
        });

        await Task.WhenAll(tasks);
    }

    public override bool CanPerform() {
        return _analog is { RelatedComponent: not null };
    }
}