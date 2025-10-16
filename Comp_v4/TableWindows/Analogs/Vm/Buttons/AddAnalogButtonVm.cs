using CommunityToolkit.Mvvm.Input;
using Comp_v4.TableWindows.Analogs.Entities;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class AddAnalogButtonVm
{
    public bool CanAdd() {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanAdd))]
    public void Add() {
        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IFormOpenHandler>(h => h?.OpenForm<AddFormState>(new Analog()));
    }
}