using System.Windows.Input;
using Comp_v4.TableWindows.Analogs._Installers;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class AnalogsTable : GenericStateMachine<BaseAnalogsTableState, AnalogsTable>, IMouseDoubleClickHandler
{
    public AnalogsTable(IEnumerable<BaseAnalogsTableState> states, BaseAnalogsTableState initialState) : base(states, initialState) {
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        CurrentState.OnMouseDoubleClick(sender, e);
    }
}

public abstract class BaseAnalogsTableState : StateBase<AnalogsTable>
{
    public abstract void OnMouseDoubleClick(object sender, MouseButtonEventArgs e);
}

public class EditAnalogsTableState : BaseAnalogsTableState
{
    protected readonly AnalogsTableVm _analogsTableVm;
    protected readonly AnalogFormManager _analogFormManager;

    public EditAnalogsTableState(AnalogsTableVm analogsTableVm, AnalogFormManager analogFormManager) {
        _analogsTableVm = analogsTableVm;
        _analogFormManager = analogFormManager;
    }

    public override void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        if (_analogsTableVm.SelectedItem is not Analog analog) 
            throw new InvalidOperationException();
        
        _analogFormManager.OpenForm<EditAnalogsFormState>(analog);
    }
}