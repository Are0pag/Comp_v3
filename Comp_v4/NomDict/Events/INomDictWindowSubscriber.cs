
namespace Comp_v4.NomDict.Events;

public interface INomDictWindowSubscriber : IDisposable { }

public interface IDataGridDoubleClickHandler : INomDictWindowSubscriber
{
    void OnDataGridDoubleClick(object? args);
}

public interface ISelectedCategoryChangedHandler : INomDictWindowSubscriber
{
    void OnSelectedCategoryChanged(object? args);
}

public interface IUiRefreshHandler : INomDictWindowSubscriber
{
    void RefreshUi(object? args);
}