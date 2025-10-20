using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils.WPF;
using WPF.UCL;

namespace Comp_v4.NomDict.Entities;

public class UpdateCategoryNameAction : UpdateCategoryAction
{
    protected readonly CategoryValidator _validator;
    protected string? _previousCategoryName;
    
    public UpdateCategoryNameAction(UpdateCategoryNameButtonVm buttonVm, TreeViewVm treeViewVm, 
                                    IRepository<Category> repository, CategoryValidator validator) : base(buttonVm, treeViewVm, repository) {
        _validator = validator;
    }

    public override async Task PerformAsync(object? parameter) {
        if (!CanPerform()) return;

        var categoryFromDb = await _repository.GetByIdAsync(_treeViewVm.SelectedCategory!.Id);
        if (categoryFromDb == null) return;

        _previousCategoryName = categoryFromDb.Name;

        var window = new OneValueWindow(valueName: "Новое имя: ",
                                        isValidCheck: s => {
                                            categoryFromDb.Name = s;
                                            return _validator.ValidateAsync(categoryFromDb).Result is { IsValid: true };
                                        },
                                        textInInputField: categoryFromDb.Name);
        WindowLocator.LocateBy(window).ShowDialog();

        await _repository.UpdateAsync(categoryFromDb);

        _treeViewVm.SelectedCategory.Name = categoryFromDb.Name;

        _treeViewVm.NotifyUiForChanges();
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}