using System.Windows.Input;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Entities;
using Templates.Common.Events.Input;
using Utils.EventBus;

namespace Comp_v4.NomDict.Entities.InputHandlers;

public class DataGridInputHandler : IMouseDoubleClickHandler
{
    protected readonly Grid _grid;
    
    public DataGridInputHandler(CardComponentManager cardComponentManager, Grid grid) {
        _grid = grid;
        EventBus<IGlobalMouseSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IGlobalMouseSubscriber>.Unsubscribe(this);
    }

    public void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        _grid.OnMouseDoubleClick(sender, e);
    }
}