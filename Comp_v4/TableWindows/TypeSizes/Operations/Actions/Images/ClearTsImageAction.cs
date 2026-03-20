using Comp_v4._Installers;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp.ModelData.TechnicalItems;
using Templates.Common.Actions.Images;
using Utils.EventBus;

namespace Comp_v4.TableWindows.TypeSizes;

public class ClearTsImageAction : ImageActionBase, IRuntimeParamsContainer<TypeSize>
{
    protected TypeSize _typeSize;
    public ClearTsImageAction(TsImageFieldVm imageFieldVm) : base(imageFieldVm) {
        _imageFieldVm.ClearAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        RuntimeParam.ImagePath = "";
        _imageFieldVm.ImagePath = null;
        _imageFieldVm.Image = null;
    }

    public override bool CanPerform() {
        return true;
    }

    public override void CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }

    public TypeSize RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<TypeSize>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return  _typeSize;
        }
        set => _typeSize = value;
    }
}