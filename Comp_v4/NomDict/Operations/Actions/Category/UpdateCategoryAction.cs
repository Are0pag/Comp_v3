using Comp_v4.NomDict.Vm;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Entities;

public abstract class UpdateCategoryAction : BaseAsyncActionButtonInvoked
{
    protected readonly TreeViewVm _treeViewVm;
    protected readonly IRepository<Category> _repository;
    public UpdateCategoryAction(BaseAsyncBButtonVm buttonVm, TreeViewVm treeViewVm, IRepository<Category> repository) : base(buttonVm) {
        _treeViewVm = treeViewVm;
        _repository = repository;
    }
}