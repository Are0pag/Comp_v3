using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v4.NomDict.Events;
using Comp.Db.Contracts;
using Comp.ModelData.SortingItems;
using Utils;
using Utils.EventBus;
using Utils.WPF.Buttons;
using Component = Comp.ModelData.Comp.Component;

namespace Comp_v4.NomDict.Vm;

public class TreeViewVm : ObservableObject, ISelectedCategoryChangedHandler, IComponentUiHandler
{
    protected readonly IRepository<Category> _repository;
    protected readonly DataGridVm _dataGridVm;
    protected Category? _selectedCategory;
    protected bool _viewSubcategoriesContent;
    
    public TreeViewVm(DataGridVm dataGridVm, IRepository<Category> repository) {
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
        _dataGridVm = dataGridVm;
        _repository = repository;
        
        _ = LoadDataAsync();
        ObserveCategoriesProps();
        
        var collectionView = CollectionViewSource.GetDefaultView(_dataGridVm.Items);
        collectionView.Filter = ItemsFilter;
    }
    
    public ObservableCollection<Category> Items { get; set; }
    public Category? SelectedCategory {
        get => _selectedCategory;
        set {
            _selectedCategory = value;
            EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
        }
    }

    public bool ViewSubcategoriesContent {
        get => _viewSubcategoriesContent;
        set {
            _viewSubcategoriesContent = value;
            OnPropertyChanged();
            CollectionViewSource.GetDefaultView(_dataGridVm.Items).Refresh();
        }
    }

    protected async Task LoadDataAsync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<Category>(items);
        OnPropertyChanged(nameof(Items));
    }

#region UpdatesUi

    public void OnSelectedCategoryChanged(object? args) {
        if (args is not TreeView treeView) 
            throw new ArgumentException();
        
        _selectedCategory = treeView.SelectedItem as Category;
        
        CollectionViewSource.GetDefaultView(_dataGridVm.Items).Refresh();
    }

    public virtual void NotifyUiForChanges() {
        Items.Clear(); //OnPropertyChanged(nameof(Items));
        _ = LoadDataAsync(); OnPropertyChanged(nameof(Items));
    }

    public void OnComponentCardCreated(object? args) {
        NotifyUiForChanges();
    }

#endregion

#region Filtering

    protected virtual bool ItemsFilter(object item) {
        if (_selectedCategory == null) return false;
        try {
            if (item is not Component component)
                return false;
            return IsSelectedCategoryIsParent(component.Category);
        }
        catch (Exception e) {
            throw new ArgumentException($"Invalid category: {_selectedCategory}", e);
        }
    }

    protected bool IsSelectedCategoryIsParent(Category category) {
        if (category.Id == _selectedCategory!.Id) 
            return true;

        if (ViewSubcategoriesContent) {
            return _selectedCategory.Subcategories
                                    .FindAllRecursive(c => c.Subcategories, c => c.Id == category.Id)
                                    .Any();
        }
        
        // не совсем понятно зачем, но пусть
        category.ParentCategory ??= Items.FirstOrDefault(c => c.Id == category.ParentCategoryId);
        return category.ParentCategory != null && IsSelectedCategoryIsParent(category.ParentCategory);
    }

#endregion

    protected void ObserveCategoriesProps() {
        foreach (var category in Items!.FindAllRecursive(c => c.Subcategories, _ => true)) {
            category.PropertyChanged += Category_PropertyChanged;
        }
    }
    
    private void Category_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(Category.IsExpanded) && sender is Category category) {
            _repository.UpdateAsync(category);
        }
    }

    public virtual void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
        
        if (Items == null) return;
    
        // В Dispose проходим по той же коллекции и отписываем метод
        foreach (var category in Items.FindAllRecursive(c => c.Subcategories, _ => true)) {
            category.PropertyChanged -= Category_PropertyChanged;
        }
    }
}