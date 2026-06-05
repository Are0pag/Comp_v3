using Comp.ModelData.Comp;

namespace Comp_v4.TableWindows.OrderPositions.Events;

public interface IOrderPositionSubscriber { }

public interface IOrderPosSavingCommitHandler : IOrderPositionSubscriber
{
    Task OnSaveOp(TaskCompletionSource tcs, object? args = null);
}

public interface IOpTableReloadHandler : IOrderPositionSubscriber, IDisposable
{
    void OnOpTableReload(object? args = null);
}

public interface IStartEditingOpHandler : IOrderPositionSubscriber, IDisposable
{
    void OnStartEditing(object? args = null);
}