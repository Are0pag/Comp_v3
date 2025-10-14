using Comp_v4.CompCard.Vm;
using Comp.ModelData;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Operations.Actions;

public class ClearImageAction : ImageActionBase
{
    public ClearImageAction(ImageFieldVmBase imageFieldVm, IImageOwner item) : base(imageFieldVm, item) {
        _imageFieldVm.ClearAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        _item.ImagePath = null;
        _imageFieldVm.ImagePath = null;
        _imageFieldVm.Image = null;
    }

    public override bool CanPerform() {
        return _item.ImagePath != null;
    }

    public override void CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}