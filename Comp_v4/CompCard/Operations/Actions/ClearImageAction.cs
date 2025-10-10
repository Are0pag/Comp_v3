using Comp_v4.CompCard.Vm;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Operations.Actions;

public class ClearImageAction : ImageActionBase
{
    protected readonly Component _component;
    
    public ClearImageAction(ImageFieldVm imageFieldVm, Component component) : base(imageFieldVm) {
        _component = component;
        _imageFieldVm.ClearAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        _component.ImagePath = null;
        _imageFieldVm.ImagePath = null;
        _imageFieldVm.Image = null;
    }

    public override bool CanPerform() {
        return _component.ImagePath != null;
    }

    public override void CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}