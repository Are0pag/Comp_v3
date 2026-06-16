using Comp.ModelData;

namespace Comp_v4.TableWindows.PaymentOrders.events;

public interface IPoSubscriber : IDisposable { }

public interface IStartEditingPo : IPoSubscriber
{
    Task OnStartEditingPo(PaymentOrder paymentOrder);
}