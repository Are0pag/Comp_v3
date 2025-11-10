using Comp_v4.CompCard.Vm;
using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.CompCard.Operations.Actions;

public abstract class ImageActionBase : BaseAction
{
    protected readonly ImageFieldVmBase _imageFieldVm;
    protected IImageOwner _item;

    protected ImageActionBase(ImageFieldVmBase imageFieldVm) {
        _imageFieldVm = imageFieldVm;
    }
}