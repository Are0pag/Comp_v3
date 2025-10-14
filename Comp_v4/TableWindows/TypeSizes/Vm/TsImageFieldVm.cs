using CommunityToolkit.Mvvm.Input;
using Comp_v4.CompCard.Vm;

namespace Comp_v4.TableWindows.TypeSizes.Vm;

public partial class TsImageFieldVm : ImageFieldVmBase
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