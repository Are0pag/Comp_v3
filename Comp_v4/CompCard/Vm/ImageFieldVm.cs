using CommunityToolkit.Mvvm.Input;
using Comp_v4.CompCard.Events;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.CompCard.Vm;

public partial class ImageFieldVm : ImageFieldVmBase, ICompCardLoadedHandler
{
    public ImageFieldVm() {
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }
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

    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public void OnCompCardLoaded(Component component) {
        ImagePath = component.ImagePath;
    }
}