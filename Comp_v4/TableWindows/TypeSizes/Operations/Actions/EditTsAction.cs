using Comp_v4.TableWindows.TypeSizes.Entities.Form;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.TypeSizes;

public class EditTsAction : ActionUpdateItem<TypeSizesTableWindow, TypeSize>
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly DataGridViewModel<TypeSize> _dataGridViewModel;
    protected readonly EditTsButVm _editTsButVm;
    protected TaskCompletionSource<BaseAction<TypeSizesTableWindow, TypeSize>>? _tcs;
    
    public EditTsAction(IDataGridCommandScheduler scheduler, 
                        ModuleContext<TypeSizesTableWindow, TypeSize> context, 
                        ICommandFactory commandFactory, 
                        IRepository<TypeSize> repository, 
                        IServiceProvider serviceProvider, 
                        DataGridViewModel<TypeSize> dataGridViewModel, 
                        EditTsButVm editTsButVm) : base(scheduler, context, commandFactory, repository) {
        _serviceProvider = serviceProvider;
        _dataGridViewModel = dataGridViewModel;
        _editTsButVm = editTsButVm;
        _editTsButVm.ClickActionAsync = ClickActionAsync;
        _editTsButVm.CanExecuteFunc = CanPerform;
    }

    protected async Task ClickActionAsync(TaskCompletionSource tcs) {
        await PerformAsync();
    }

    public override async Task<BaseAction<TypeSizesTableWindow, TypeSize>> PerformAsync(object? parameter = null) {
        _tcs = new TaskCompletionSource<BaseAction<TypeSizesTableWindow, TypeSize>>();
        var window = ActivatorUtilities.CreateInstance<TsFormWindow>(_serviceProvider, _dataGridViewModel.SelectedItem!);

        var form = _serviceProvider.GetRequiredService<FormTs>();
        await form.ChangeState(form.GetState<EditItemTsStateForm>(), form);

        _serviceProvider.GetRequiredService<SelectTypeSizeImageAction>();
        _serviceProvider.GetRequiredService<OpenTsImageAction>();
        _serviceProvider.GetRequiredService<ClearTsImageAction>();
        
        window.Closed += (sender, args) => {
            _tcs.SetResult(this);
        };
        window.Show();
        
        var res = await _tcs.Task;
        _tcs = null;
        return res;
    }

    public override bool CanPerform() {
        return _dataGridViewModel.SelectedItem != null;
    }
}