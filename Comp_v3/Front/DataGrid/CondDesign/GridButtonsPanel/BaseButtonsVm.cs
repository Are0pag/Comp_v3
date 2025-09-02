using CommunityToolkit.Mvvm.ComponentModel;

namespace Comp_v3.Front.DataGrid.CondDesign.Entities;

public partial class BaseButtonsVm : ObservableObject
{
    protected readonly CognDesignGridVm _condDesignGridVm;
    
    public BaseButtonsVm(CognDesignGridVm condDesignGridVm) {
        _condDesignGridVm = condDesignGridVm;
    }
    
    public virtual void Dispose() {
        
    }
}