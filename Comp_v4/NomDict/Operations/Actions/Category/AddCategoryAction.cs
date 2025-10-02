using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils.WPF;
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

        // Получаем свежий экземпляр из БД чтобы избежать проблем с отслеживанием
        var selectedCategoryFromDb = await _repository.GetByIdAsync(_treeViewVm.SelectedCategory.Id);
        if (selectedCategoryFromDb == null) 
            return;

        _creatingCategory = new Category();
        
        var window = new OneValueWindow("Новая категория: ", s => {
            _creatingCategory.Name = s;
            return _validator.ValidateAsync(_creatingCategory).Result is { IsValid: true };
        });
        WindowLocator.LocateBy(window).ShowDialog();

        _creatingCategory.Id = default;
        _creatingCategory.ParentCategory = selectedCategoryFromDb;
        _creatingCategory.ParentCategoryId = selectedCategoryFromDb.Id;
        
        await _repository.AddAsync(_creatingCategory);

        selectedCategoryFromDb.AddSubcategory(_creatingCategory);

        if (!selectedCategoryFromDb.IsExpanded) {
            selectedCategoryFromDb.IsExpanded = true;
            await _repository.UpdateAsync(selectedCategoryFromDb);
        }

        _treeViewVm.SelectedCategory = _creatingCategory;
        
        _creatingCategory = null;

        _selectedCategory.IsExpanded = true;
        _treeViewVm.NotifyUiForChanges();
        _selectedCategory = null;
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}