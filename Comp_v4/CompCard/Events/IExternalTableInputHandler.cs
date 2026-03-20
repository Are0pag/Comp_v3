using Comp.ModelData.TechnicalItems;

namespace Comp_v4.CompCard.Events;

public interface IExternalTableInputHandler<T> : ICompCardSubscriber
    where T : class, IDbEntity, new()
{
    void HandleTableInput(T? args);
}