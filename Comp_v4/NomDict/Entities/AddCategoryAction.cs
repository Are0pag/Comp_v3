using Comp_v4.NomDict.Vm;
using Comp.ModelData.SortingItems;
using Utils.WPF;

namespace Comp_v4.NomDict.Entities;

public class AddCategoryAction : BaseAction
{
    protected readonly TreeViewVm _treeViewVm;

    public AddCategoryAction(TreeViewVm treeViewVm) {
        _treeViewVm = treeViewVm;
    }

    public override async Task PerformAsync(object? parameter = null) {
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