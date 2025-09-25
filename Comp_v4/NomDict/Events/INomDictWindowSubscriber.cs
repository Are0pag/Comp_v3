
namespace Comp_v4.NomDict.Events;

public interface INomDictWindowSubscriber { }

public interface IDataGridDoubleClickHandler : INomDictWindowSubscriber
{
    void OnDataGridDoubleClick(object? args);
}