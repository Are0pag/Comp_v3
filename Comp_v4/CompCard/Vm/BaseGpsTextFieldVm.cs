using Comp_v4.CompCard.Events;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseGpsTextFieldVm : BaseTextFieldVm, IExternalTableInputHandler<GenericParametersSet>
{
    /// <summary>
    /// Key - потому что это ключ, а не значение
    /// </summary>
    public const string DEFAULT_GPS_KEY = "Не выбрано";
    protected BaseGpsTextFieldVm(StringValidatorBase validator) : base(validator) {
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }

    public override void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public abstract void HandleTableInput(GenericParametersSet? args);
}