using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils.WPF;
using Utils.WPF.Buttons;
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
        _previousCategoryName = _treeViewVm.SelectedCategory!.Name;
        var window = new OneValueWindow(valueName: "Новое имя: ",
                                        isValidCheck: s => {
                                            _treeViewVm.SelectedCategory!.Name = s;
                                            return _validator.ValidateAsync(_treeViewVm.SelectedCategory).Result is { IsValid: true };
                                        },
                                        textInInputField: _treeViewVm.SelectedCategory!.Name);
        WindowLocator.LocateBy(window).ShowDialog();
        
        await _repository.UpdateAsync(_treeViewVm.SelectedCategory!);
    }

    public override bool CanPerform() {
        return _treeViewVm.SelectedCategory != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}