using System.Windows.Input;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;

namespace Comp_v4.TableWindows.TypeSizes.Events;

public interface ITypeSizesWindowSubscriber : IDisposable { }

public interface ITypeSizeFormOpenHandler : ITypeSizesWindowSubscriber
{
    void OpenTsForm<T>(object? parameter = null) where T : BaseTsStateForm;
}

public interface ITypeSizeCreateHandler : ITypeSizesWindowSubscriber
{
    Task OnCreate(object? parameter = null);
}

public interface IMouseDoubleClickHandler : ITypeSizesWindowSubscriber
{
    void OnMouseDoubleClick(object sender, MouseButtonEventArgs e);
}