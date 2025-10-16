using System.Windows.Input;
using Comp_v4.TableWindows.Analogs.Entities;

namespace Comp_v4.TableWindows.Analogs.Events;

public interface IAnalogsTableWindowSubscriber : IDisposable
{
    
}

public interface IFormOpenHandler : IAnalogsTableWindowSubscriber
{
    void OpenForm<T>(object? parameter = null) where T : BaseFormState;
}

public interface IMouseDoubleClickHandler : IAnalogsTableWindowSubscriber
{
    void OnMouseDoubleClick(object sender, MouseButtonEventArgs e);
}

public interface ISelectAnalogHandler : IAnalogsTableWindowSubscriber
{
    void OnStartSelectingAnalog(object? parameter = null);
}