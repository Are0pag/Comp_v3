namespace Comp_v4.TableWindows.TypeSizes.Events;

public interface ITypeSizesWindowSubscriber : IDisposable { }

public interface ITypeSizeFormOpenHandler : ITypeSizesWindowSubscriber
{
    void OpenTsForm(object? parameter = null);
}