using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.StateMachine;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.TypeSizes.Entities.Form.States;

public abstract class BaseTsStateForm : StateBase<FormTs>
{
    protected readonly IRepository<TypeSize> _repository;
    protected readonly DataGridViewModel<TypeSize> _dataGridViewModel;

    protected BaseTsStateForm(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) {
        _repository = repository;
        _dataGridViewModel = dataGridViewModel;
    }
    public abstract Task OnCreate(object? parameter = null);
}

public class AddItemTsStateForm : BaseTsStateForm
{
    public AddItemTsStateForm(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) : base(repository, dataGridViewModel) {
        Console.WriteLine("AddItemStateForm");
    }

    public override async Task OnCreate(object? parameter = null) {
        if (parameter is not TypeSize newTypeSize)
            throw new InvalidOperationException();

        try {
            await _repository.AddAsync(newTypeSize);
        }
        catch (Exception exception) {
            Console.WriteLine(exception.Message);
        }
        
        _dataGridViewModel.Items.Add(newTypeSize);
        _dataGridViewModel.SelectedItem = newTypeSize;
    }
}

public class EditItemTsStateForm : BaseTsStateForm
{
    public EditItemTsStateForm(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) : base(repository, dataGridViewModel) {
        Console.WriteLine("AddItemStateForm");
    }

    public override async Task OnCreate(object? parameter = null) {
        if (parameter is not TypeSize newTypeSize)
            throw new InvalidOperationException();

        try {
            await _repository.UpdateAsync(newTypeSize);
        }
        catch (Exception exception) {
            Console.WriteLine(exception.Message);
        }
    }
}