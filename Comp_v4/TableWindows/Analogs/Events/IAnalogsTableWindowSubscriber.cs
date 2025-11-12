using System.Windows.Input;
using Comp_v4.TableWindows.Analogs.Entities;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Analogs.Events;

public interface IAnalogsTableWindowSubscriber : IDisposable
{
    
}

public interface IFormOpenHandler : IAnalogsTableWindowSubscriber
{
    void OpenForm<T>(object? parameter = null) where T : BaseAnalogsFormState;
}

public interface IMouseDoubleClickHandler : IAnalogsTableWindowSubscriber
{
    void OnMouseDoubleClick(object sender, MouseButtonEventArgs e);
}

public interface ISelectAnalogHandler : IAnalogsTableWindowSubscriber
{
    Task OnStartSelectingAnalog(object? parameter = null);
}

public interface IAnalogSaveHandler : IAnalogsTableWindowSubscriber
{
    Task Save(TaskCompletionSource tcs, Analog analog);
}

public interface IAnalogTableLoadHandler : IAnalogsTableWindowSubscriber
{
    Task OnLoad(TaskCompletionSource tcs);
}