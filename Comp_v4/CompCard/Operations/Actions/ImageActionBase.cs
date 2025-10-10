using Comp_v4.CompCard.Vm;
using Utils.WPF;

namespace Comp_v4.CompCard.Operations.Actions;

public abstract class ImageActionBase : BaseAction
{
    protected readonly ImageFieldVm _imageFieldVm;

    protected ImageActionBase(ImageFieldVm imageFieldVm) {
        _imageFieldVm = imageFieldVm;
    }
}