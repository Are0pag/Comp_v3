using System.Windows.Input;
using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;

namespace Comp_v4.NomDict.Entities;

public class EditGridState : BaseSGridState
{
    protected readonly CardComponentManager _cardComponentManager;
    protected readonly TreeViewVm _treeViewVm;

    public EditGridState(CardComponentManager cardComponentManager, TreeViewVm treeViewVm) {
        _cardComponentManager = cardComponentManager;
        _treeViewVm = treeViewVm;
    }

    public override void OnMouseDoubleClick(object sender, MouseButtonEventArgs e, Grid context) {
        if (sender is not Component component)
            throw new ArgumentException();
        
        _cardComponentManager.OpenWindow<EditStateCardComp>(new CardComponentManager.Args(component));
    }

    public override void AddComponent(object? parameter) {
        if (_treeViewVm.SelectedCategory == null)
            throw new ArgumentException();
        
        _cardComponentManager.OpenWindow<CreateStateCardComp>(new CardComponentManager.Args(
                                                                   new Component(),
                                                                   _treeViewVm.SelectedCategory
                                                                  ));
    }
}