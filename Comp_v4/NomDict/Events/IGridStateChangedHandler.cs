namespace Comp_v4.NomDict.Events;

public interface IGridSelectingStateHandler : INomDictWindowSubscriber
{
    void OnSelecting(object? parameter = null);
}