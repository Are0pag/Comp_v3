using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils.WPF;
using Utils.WPF.Buttons;
using WPF.UCL;

namespace Comp_v4.NomDict.Entities;

public class AddCategoryAction : BaseAsyncActionButtonInvoked
{
    protected readonly TreeViewVm _treeViewVm;
    protected readonly CategoryValidator _validator;
    protected readonly IRepository<Category> _repository;
    protected Category? _category;

    public AddCategoryAction(AddNewCategoryButtonVm buttonVm, TreeViewVm treeViewVm, CategoryValidator validator, IRepository<Category> repository) : base(buttonVm) {
        _treeViewVm = treeViewVm;
        _validator = validator;
        _repository = repository;
    }

    public override async Task PerformAsync(object? parameter) {
        _category = new Category();
        var window = new OneValueWindow("Новая категория: ", s => {
            _category.Name = s;
            return _validator.ValidateAsync(_category).Result is { IsValid: true };
        });
        WindowLocator.LocateBy(window).ShowDialog();

        _category.ParentCategory = _treeViewVm.SelectedCategory;

        await _repository.AddAsync(_category);
        
        _treeViewVm.SelectedCategory!.AddSubcategory(_category);
        _treeViewVm.SelectedCategory = _category;
        _category = null;
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}