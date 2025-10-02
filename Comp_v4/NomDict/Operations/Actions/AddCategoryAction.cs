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
    protected readonly IRepository<Category> _repository;
    protected readonly CategoryValidator _validator;
    protected Category? _creatingCategory;
    protected Category? _selectedCategory;

    public AddCategoryAction(AddNewCategoryButtonVm buttonVm, TreeViewVm treeViewVm, CategoryValidator validator, IRepository<Category> repository) : base(buttonVm) {
        _treeViewVm = treeViewVm;
        _validator = validator;
        _repository = repository;
    }

    public override async Task PerformAsync(object? parameter) {
        if (_treeViewVm.SelectedCategory == null) 
            return;
        _selectedCategory = _treeViewVm.SelectedCategory;
        _creatingCategory = new Category();
        var window = new OneValueWindow("Новая категория: ", s => {
            _creatingCategory.Name = s;
            return _validator.ValidateAsync(_creatingCategory).Result is { IsValid: true };
        });
        WindowLocator.LocateBy(window).ShowDialog();

        _creatingCategory.Id = default;
        await _repository.AddAsync(_creatingCategory);

        _treeViewVm.SelectedCategory!.IsExpanded = true;
        _creatingCategory.ParentCategory = _treeViewVm.SelectedCategory;
        _treeViewVm.SelectedCategory!.AddSubcategory(_creatingCategory);
        await _repository.UpdateAsync(_creatingCategory);
        await _repository.UpdateAsync(_treeViewVm.SelectedCategory);

        _treeViewVm.SelectedCategory = _creatingCategory;
        _creatingCategory = null;
        _selectedCategory = null;
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}