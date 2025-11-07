using System.Windows.Input;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Counterparties.Events;

public interface ICounterpartySubscriber : IDisposable { }

public interface ISaveHandler : ICounterpartySubscriber
{
    Task Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null);
}

public interface IMouseDoubleClickHandler : ICounterpartySubscriber
{
    Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs e);
}

public interface ICpFormOnSaveUiChangesHandler : ICounterpartySubscriber
{
    Task OnSaveCpForm(TaskCompletionSource tcs, object? parameter = null);
}

public interface ISelectionConfirmationHandler : ICounterpartySubscriber
{
    Task OnConfirmSelection(TaskCompletionSource tcs, object parameter = null);
}