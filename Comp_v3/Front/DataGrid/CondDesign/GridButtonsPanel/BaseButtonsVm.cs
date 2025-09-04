using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.Events.Buttons;
using Utils.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public abstract class BaseButtonsVm : ObservableObject, INotifyConditionalsChanged
{
    protected readonly CognDesignGridVm _condDesignGridVm;
    
    public BaseButtonsVm(CognDesignGridVm condDesignGridVm) {
        _condDesignGridVm = condDesignGridVm;
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }
    
    public virtual void Dispose() {
        EventBus<IGlobalButtonEvent>.Unsubscribe(this);
    }

    public abstract void NotifyCanExecute();
}