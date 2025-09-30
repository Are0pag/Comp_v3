using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v4.NomDict.Events;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Utils.EventBus;

namespace Comp_v4.NomDict.Vm;

public class TreeViewVm : ObservableObject, ISelectedCategoryChangedHandler
{
    protected readonly IRepository<Category> _repository;
    protected readonly DataGridVm _dataGridVm;
    protected Category? _selectedCategory;
    
    public TreeViewVm(DataGridVm dataGridVm, IRepository<Category> repository) {
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
        _dataGridVm = dataGridVm;
        _repository = repository;
        _ = LoadDataSync();
        var collectionView = CollectionViewSource.GetDefaultView(_dataGridVm.Items);
        collectionView.Filter = ItemsFilter;
    }
    
    public ObservableCollection<Category> Items { get; set; }
    public Category? SelectedCategory {
        get => _selectedCategory;
        set => _selectedCategory = value;
    }

    protected async Task LoadDataSync() {
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<Category>(items);
        OnPropertyChanged(nameof(Items));
    }

    protected virtual bool ItemsFilter(object item) {
        return _selectedCategory != null && ((Component) item).Category == _selectedCategory;
    }
    
    public void OnSelectedCategoryChanged(object? args) {
        if (args is not TreeView treeView) 
            throw new ArgumentException();
        
        _selectedCategory = treeView.SelectedItem as Category;
        
        CollectionViewSource.GetDefaultView(_dataGridVm.Items).Refresh();
    }

    public virtual void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
    }
}