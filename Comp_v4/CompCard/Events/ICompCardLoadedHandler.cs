using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Events;

public interface ICompCardLoadedHandler : ICompCardSubscriber
{
    void OnCompCardLoaded(Component component);
}