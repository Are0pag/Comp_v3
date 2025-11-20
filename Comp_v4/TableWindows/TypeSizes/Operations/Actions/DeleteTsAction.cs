using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.TableWindows.TypeSizes;

public class DeleteTsAction : ActionDeleteItem<TypeSizesTableWindow, TypeSize>
{
    protected readonly ActionSave<TypeSizesTableWindow, TypeSize> _actionSave;
    public DeleteTsAction(IDataGridCommandScheduler scheduler, 
                          ModuleContext<TypeSizesTableWindow, TypeSize> context, 
                          ICommandFactory commandFactory, Cell<TypeSizesTableWindow, 
                              TypeSize> cell, IRepository<TypeSize> repository, 
                          ActionSave<TypeSizesTableWindow, TypeSize> actionSave) 
        : base(scheduler, context, commandFactory, cell, repository) {
        _actionSave = actionSave;
    }

    public override async Task<BaseAction<TypeSizesTableWindow, TypeSize>> PerformAsync(object? parameter = null) {
        await base.PerformAsync(parameter);
        await _actionSave.PerformAsync();
        return this;
    }
}