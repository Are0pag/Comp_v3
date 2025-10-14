using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.CompCard.Vm;

public partial class ImageFieldVm : ImageFieldVmBase
{
    [RelayCommand]
    public override void Clear() {
        base.Clear();
    }

    [RelayCommand]
    public override void Open() {
        base.Open();
    }

    [RelayCommand]
    public override void Select() {
        base.Select();
    }
}