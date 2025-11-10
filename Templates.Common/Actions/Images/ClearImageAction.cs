using Comp_v4._Installers;
using Comp_v4.CompCard.Vm;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.CompCard.Operations.Actions;

public class ClearImageAction : ImageActionBase, IRuntimeParamsContainer<IImageOwner>
{
    public ClearImageAction(ImageFieldVmBase imageFieldVm) : base(imageFieldVm) {
        _imageFieldVm.ClearAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        RuntimeParam.ImagePath = null;
        _imageFieldVm.ImagePath = null;
        _imageFieldVm.Image = null;
    }

    public override bool CanPerform() {
        return RuntimeParam.ImagePath != null;
    }

    public override void CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
    
    public IImageOwner RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<IImageOwner>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _item;
        }
        set => _item = value;
    }
}