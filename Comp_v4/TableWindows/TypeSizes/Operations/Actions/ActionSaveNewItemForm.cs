using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.TypeSizes;

public class ActionSaveNewItemForm
{
    protected readonly TypeSize _item;
    protected readonly ValidatorBase<TypeSize> _validator; 
    
    public ActionSaveNewItemForm(TypeSize item, ValidatorBase<TypeSize> validator) {
        _item = item;
        _validator = validator;
    }

    public void Perform(object? parameter = null) {
        EventBus<ITypeSizesWindowSubscriber>.RaiseEvent<ITypeSizeCreateHandler>(h => h?.OnCreate(_item));
    }

    public bool CanPerform() {
        return _validator.ValidateAsync(_item).Result is {IsValid: true};
    }
}