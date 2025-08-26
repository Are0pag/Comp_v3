using System.Collections.ObjectModel;
using Utils.WPF.Mvvm;

namespace CL_Comp_ModelData.SortingItems;

public class Category : NotifyPropertyChanged
{
    public Category(string name, Category? parent) {
        Name.Value = name;
        ParentCategory = parent;
    }

    public int Id { get; set; }
    public ObservableModelProperty<string> Name { get; set; } = new();

    public ObservableModelProperty<bool> IsExpanded { get; set; } = new();

    public Category? ParentCategory { get; set; }

    public ObservableCollection<Category>? Subcategories {get; set; }

    public void AddSubcategory(Category subcategory) {
        Subcategories ??= new ();
        Subcategories.Add(subcategory);
    }

    public override string ToString() => GetFullNameRecursive(this, Name.Value);

    public string GetFullNameRecursive(Category category, string name) {
        return category.ParentCategory != null ? GetFullNameRecursive(category.ParentCategory, category.ParentCategory.Name.Value + "/" + name) : name;
    }
}
