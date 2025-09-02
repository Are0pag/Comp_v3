using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v3.Front.DataGrid.CondDesign.Grid;

namespace Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;

public partial class BaseButtonsVm : ObservableObject
{
    protected readonly CognDesignGridVm _condDesignGridVm;
    
    public BaseButtonsVm(CognDesignGridVm condDesignGridVm) {
        _condDesignGridVm = condDesignGridVm;
    }
    
    public virtual void Dispose() {
        
    }
}