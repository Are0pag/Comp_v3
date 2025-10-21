using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Counterparties.Form.Events;

public interface ICounterpartySubscriber : IDisposable { }

public interface ICounterpartyFormHandler : ICounterpartySubscriber
{
    void Open<T>(TaskCompletionSource tcs, object? parameter = null) where T : BaseFormState;
}

public interface ISaveHandler : ICounterpartySubscriber
{
    void Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null);
}