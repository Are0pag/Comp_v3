using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp_v4.TableWindows.Analogs.Entities;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class AddAnalogButtonVm : ObservableObject
{
    protected string _label = "Добавить";

    public string Label {
        get => _label;
        set {
            _label = value;
            OnPropertyChanged();
        }
    }
    
    public bool CanAdd() {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanAdd))]
    public void Add() {
        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IFormOpenHandler>(h => h?.OpenForm<AddFormState>(new Analog()));
    }
}