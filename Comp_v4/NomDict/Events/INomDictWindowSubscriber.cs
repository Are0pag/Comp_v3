
namespace Comp_v4.NomDict.Events;

public interface INomDictWindowSubscriber : IDisposable { }


public interface ISelectedCategoryChangedHandler : INomDictWindowSubscriber
{
    void OnSelectedCategoryChanged(object? args);
}

public interface IComponentUiHandler : INomDictWindowSubscriber
{
    void OnComponentCardCreated(object? args);
}