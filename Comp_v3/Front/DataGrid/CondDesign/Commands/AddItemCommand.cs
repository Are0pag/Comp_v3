using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Infrastructure.Command.Classic;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class AddItemCommand : ICommand
{
    private readonly IConditionalDesignationRepository _repository;
    private readonly ObservableCollection<ConditionalDesignation?> _items;
    private readonly ConditionalDesignation _item;
    private int _insertedIndex;

    public AddItemCommand(IConditionalDesignationRepository repository, 
                          ObservableCollection<ConditionalDesignation?> items, 
                          ConditionalDesignation item)
    {
        _repository = repository;
        _items = items;
        _item = item;
    }

    public async Task ExecuteAsync() {
        await _repository.AddAsync(_item);
        _insertedIndex = _items.Count;
        _items.Add(_item);
    }

    public async Task UndoAsync() {
        await _repository.DeleteAsync(_item.Id);
        _items.Remove(_item);
    }
}