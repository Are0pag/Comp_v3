using Comp_v4._Installers;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.TypeSizes;

public class ActionSaveNewTsForm : IRuntimeParamsContainer<TypeSize>
{
    protected TypeSize _item;
    protected readonly ValidatorBase<TypeSize> _validator; 
    
    public ActionSaveNewTsForm(ValidatorBase<TypeSize> validator) {
        _validator = validator;
    }

    public void Perform(object? parameter = null) {
        EventBus<ITypeSizesWindowSubscriber>.RaiseEvent<ITypeSizeCreateHandler>(h => h?.OnCreate(RuntimeParam));
    }

    public bool CanPerform() {
        return _validator.ValidateAsync(RuntimeParam).Result is {IsValid: true};
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
            return _item;
        }
        set {
            _item = value;
        }
    }
}