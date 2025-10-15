using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.StateMachine;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.TypeSizes.Entities.Form.States;

public abstract class BaseStateForm : StateBase<Form>
{
    protected readonly IRepository<TypeSize> _repository;
    protected readonly DataGridViewModel<TypeSize> _dataGridViewModel;

    protected BaseStateForm(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) {
        _repository = repository;
        _dataGridViewModel = dataGridViewModel;
    }
    public abstract Task OnCreate(object? parameter = null);
}

public class AddItemStateForm : BaseStateForm
{
    public AddItemStateForm(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) : base(repository, dataGridViewModel) {
        Console.WriteLine("AddItemStateForm");
    }

    public override async Task OnCreate(object? parameter = null) {
        if (parameter is not TypeSize newTypeSize)
            throw new InvalidOperationException();

        await _repository.AddAsync(newTypeSize);
        
        _dataGridViewModel.Items.Add(newTypeSize);
        _dataGridViewModel.SelectedItem = newTypeSize;
    }
}

public class EditItemStateForm : BaseStateForm
{
    public EditItemStateForm(IRepository<TypeSize> repository, DataGridViewModel<TypeSize> dataGridViewModel) : base(repository, dataGridViewModel) {
        Console.WriteLine("AddItemStateForm");
    }

    public override async Task OnCreate(object? parameter = null) {
        if (parameter is not TypeSize newTypeSize)
            throw new InvalidOperationException();
        
        await _repository.UpdateAsync(newTypeSize);
    }
}