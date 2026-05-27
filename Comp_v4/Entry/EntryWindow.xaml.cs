using System.Windows;
using Comp_v4._Installers;
using Comp_v4.Entry.Vm.Buts;
using Utils.EventBus;

namespace Comp_v4.Entry;

public partial class EntryWindow : Window, IDisposable, IRuntimeParamsResolver<EntryWindow>
{
    public EntryWindow(NomDictButVm nomDictButVm, OrdersButVm ordersButVm) {
        InitializeComponent();
        NomWindowButton.DataContext = nomDictButVm;
        OrdersButton.DataContext = ordersButVm;
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<EntryWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }
}