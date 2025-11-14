using Comp_v4.TableWindows.TypeSizes.Entities.Form;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.TableWindows.TypeSizes;

public class EditTsAction : ActionUpdateItem<TypeSizesTableWindow, TypeSize>
{
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource<BaseAction<TypeSizesTableWindow, TypeSize>>? _tcs;
    
    public EditTsAction(IDataGridCommandScheduler scheduler, ModuleContext<TypeSizesTableWindow, TypeSize> context, ICommandFactory commandFactory, IRepository<TypeSize> repository, IServiceProvider serviceProvider) : base(scheduler, context, commandFactory, repository) {
        _serviceProvider = serviceProvider;
    }
    
    public override async Task<BaseAction<TypeSizesTableWindow, TypeSize>> PerformAsync(object? parameter = null) {
        _tcs = new TaskCompletionSource<BaseAction<TypeSizesTableWindow, TypeSize>>();
        var window = ActivatorUtilities.CreateInstance<TsFormWindow>(_serviceProvider, new TypeSize());

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
}