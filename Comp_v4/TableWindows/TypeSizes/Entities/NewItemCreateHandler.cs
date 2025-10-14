using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.TypeSizes.Entities;

public class NewItemCreateHandler : ITypeSizeCreateHandler
{
    protected readonly IRepository<TypeSize> _repository;
    protected readonly DataGridViewModel<TypeSize> _dataGridViewModel;
    
    public NewItemCreateHandler(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) {
        _repository = repository;
        _dataGridViewModel = dataGridViewModel;
        EventBus<ITypeSizesWindowSubscriber>.Subscribe(this);
    }
    
    public void Dispose() {
        EventBus<ITypeSizesWindowSubscriber>.Unsubscribe(this);
    }

    public async Task OnCreate(object? parameter = null) {
        if (parameter is not TypeSize newTypeSize)
            throw new InvalidOperationException();

        await _repository.AddAsync(newTypeSize);
        
        _dataGridViewModel.Items.Add(newTypeSize);
        _dataGridViewModel.SelectedItem = newTypeSize;
    }
}