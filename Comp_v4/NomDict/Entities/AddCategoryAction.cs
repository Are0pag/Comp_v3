using Comp_v4.NomDict.Vm;
using Comp.ModelData.SortingItems;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Entities;

public class AddCategoryAction : BaseAsyncActionButtonInvoked
{
    protected readonly TreeViewVm _treeViewVm;

    public AddCategoryAction(BaseAsyncBButtonVm buttonVm, TreeViewVm treeViewVm) : base(buttonVm) {
        _treeViewVm = treeViewVm;
    }

    public override async Task PerformAsync(object? parameter) {
        if (parameter is not Category subCategory)
            throw new InvalidCastException();
        
        _treeViewVm.SelectedCategory!.AddSubcategory(subCategory);
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}