using System.Windows.Input;

namespace Comp_v3.Front.Events.ViewInvoking.Keys;

public interface IPreviewKeyDownHandler : IUiGlobalSubscriber
{
    Task HandleKeyInput(object? sender, KeyEventArgs e);
}