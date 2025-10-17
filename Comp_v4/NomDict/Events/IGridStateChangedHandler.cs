using Comp.ModelData.Comp;

namespace Comp_v4.NomDict.Events;

public interface IGridSelectingStateHandler : INomDictWindowSubscriber
{
    void OnSelecting(TaskCompletionSource<Component> tcs);
}