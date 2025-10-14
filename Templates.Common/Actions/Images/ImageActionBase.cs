using Comp_v4.CompCard.Vm;
using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.CompCard.Operations.Actions;

public abstract class ImageActionBase : BaseAction
{
    protected readonly ImageFieldVm _imageFieldVm;
    protected readonly IImageOwner _item;

    protected ImageActionBase(ImageFieldVm imageFieldVm, IImageOwner item) {
        _imageFieldVm = imageFieldVm;
        _item = item;
    }
}