using Comp.ModelData.Comp;

namespace Comp_v4.NomDict.Events;

/// <summary>
/// Интерфейс обработки начала процесса выбора компонента
/// </summary>
public interface IGridSelectingStateHandler : INomDictWindowSubscriber
{
    void OnSelecting(TaskCompletionSource<Component> tcs, Type requesterType);
}