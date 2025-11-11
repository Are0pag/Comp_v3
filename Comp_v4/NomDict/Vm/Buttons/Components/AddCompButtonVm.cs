using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Vm.Buttons.Components;

public partial class AddCompButtonVm : BaseButtonAdvanced
{
    public const string COMMON_LABEL = "Новый компонент";
    public const string ANALOG_CHOOSE_LABEL = "Выбрать как аналог";
    
    protected string _label = COMMON_LABEL;
    
    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand?.NotifyCanExecuteChanged();
    }

    public string Label {
        get => _label;
        set {
            _label = value;
            OnPropertyChanged();
        }
    }
}