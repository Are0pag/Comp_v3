using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.SortingItems;

[Table(nameof(Category) + "s")]
public class Category : NotifyPropertyChanged, IPopulatable<Category>
{
    protected string _name = string.Empty;
    protected bool _isExpanded;
    
    public Category() { }
    public Category(string name, Category? parent) {
        Name = name;
        IsExpanded = false;
        ParentCategory = parent;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 

    [Required]
    public string Name {
        get => _name;
        set {
            if (_name == value) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public bool IsExpanded {
        get => _isExpanded;
        set {
            if (_isExpanded == value) return;
            _isExpanded = value;
            OnPropertyChanged();
        }
    }

    // Внешний ключ для родительской категории (самореференсная связь)
    public int? ParentCategoryId { get; set; }

    // Навигационное свойство для родительской категории
    [ForeignKey(nameof(ParentCategoryId))] 
    public Category? ParentCategory { get; set; }

    public virtual ObservableCollection<Category> Subcategories { get; set; } = new ();

    public void AddSubcategory(Category subcategory) {
        Subcategories ??= [];
        Subcategories.Add(subcategory);
    }
    
    public void RemoveSubcategory(Category subcategory) {
        Subcategories.Remove(subcategory);
        subcategory.ParentCategory = null;
        subcategory.ParentCategoryId = null;
    }

    public override string ToString() => GetFullNameRecursive(this, Name);

    public string GetFullNameRecursive(Category category, string name) {
        return category.ParentCategory != null ? GetFullNameRecursive(category.ParentCategory, category.ParentCategory.Name + "/" + name) : name;
    }

    public Category PopulateFrom(Category targetValues) {
        Id = targetValues.Id;
        Name = targetValues.Name;
        ParentCategoryId = targetValues.ParentCategoryId;
        IsExpanded = targetValues.IsExpanded;
        return this;
    }
}
