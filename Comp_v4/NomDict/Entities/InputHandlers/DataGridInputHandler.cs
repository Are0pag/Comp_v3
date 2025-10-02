
using System.Windows.Input;
using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp.ModelData.Comp;
using Templates.Common.Events.Input;
using Utils.EventBus;

namespace Comp_v4.NomDict.Entities.InputHandlers;

public class DataGridInputHandler : IMouseDoubleClickHandler
{
    protected readonly CardComponentManager _cardComponentManager;
    
    public DataGridInputHandler(CardComponentManager cardComponentManager) {
        _cardComponentManager = cardComponentManager;

        EventBus<IGlobalMouseSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IGlobalMouseSubscriber>.Unsubscribe(this);
    }

    public void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        if (sender is not Component component)
            throw new ArgumentException();
        
        _cardComponentManager.OpenWindow<EditStateCardComp>(component);
    }
}