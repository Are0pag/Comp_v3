using System.Collections.ObjectModel;
using Utils.WPF;

namespace CL_Comp_ModelData.SortingItems;

public class Category : NotifyPropertyChanged
{
    protected string _categoryName;
    protected bool _isExpanded;

    public Category(string name, Category? parent) {
        Name = name;
        ParentCategory = parent;
    }

    public int Id { get; set; }
    
    public string Name {
        get => _categoryName;
        set {
            if (value == _categoryName) return;
            _categoryName = value;
            OnPropertyChanged();
        }
    }

    public bool IsExpanded {
        get => _isExpanded;
        set {
            if (value == _isExpanded) return;
            _isExpanded = value;
            OnPropertyChanged();
        }
    }

    public string FullName => GetFullName(this, Name);
    public Category? ParentCategory { get; set; }

    public ObservableCollection<Category>? Subcategories {get; set; }

    public void AddSubcategory(Category subcategory) {
        Subcategories ??= new ();
        Subcategories.Add(subcategory);
    }

    public override string ToString() => GetFullName(this, Name);

    private string GetFullName(Category category, string name) {
        if (category.ParentCategory != null) {
            return GetFullName(category.ParentCategory, category.ParentCategory.Name + "/" + name);
        }
        return name;
    }
}
