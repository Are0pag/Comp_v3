using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;
using Utils.WPF.Dialogs;

namespace Comp_v4.TableWindows.Analogs.Actions;

public class DeleteAnalogAction : BaseActionAsyncSelfWaiting
{
    protected readonly AnalogsTableVm _analogsTableVm;
    protected readonly IRepository<Analog> _repository;
    
    public DeleteAnalogAction(DeleteAnalogButVm button, AnalogsTableVm analogsTableVm, IRepository<Analog> repository) : base(button) {
        _analogsTableVm = analogsTableVm;
        _repository = repository;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        var selectedItem = _analogsTableVm.SelectedItem!;
        var message = $"пару аналогов: {selectedItem.SourceComponent} и {selectedItem.RelatedComponent}";
        var isConfirmed = DialogService.ShowConfirmation("Удаление", $"Вы уверены что хотите удалить {message}?");
        if (!isConfirmed) return;

        try {
            await _repository.DeleteAsync(selectedItem.Id);
        }
        catch (Exception ex) {
            await Console.Error.WriteLineAsync(ex.Message);
            throw;
        }

        _analogsTableVm.Items.Remove(selectedItem);
        var tcsUi = new TaskCompletionSource();
        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IAnalogDeleteHandler>(handler => handler?.OnDeleteAnalog(tcsUi, selectedItem));
        await tcs.Task;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _analogsTableVm.SelectedItem != null;
    }
}