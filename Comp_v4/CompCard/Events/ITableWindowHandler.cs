using System.Windows;

namespace Comp_v4.CompCard.Events;

public interface ITableWindowHandler : ICompCardSubscriber
{
    void HandleClosingTableWindow<T>(object? args) where T : Window;
}