using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.TableWindows.TypeSizes;

public class ActionAddingNewItem : ActionStartAddingNewItem<TypeSizesTableWindow, TypeSize>
{
    public ActionAddingNewItem(IDataGridCommandScheduler scheduler, 
                               ModuleContext<TypeSizesTableWindow, TypeSize> context, 
                               ICommandFactory commandFactory, 
                               Cell<TypeSizesTableWindow, TypeSize> cell) 
        : base(scheduler, context, commandFactory, cell) {
    }

    public override Task<BaseAction<TypeSizesTableWindow, TypeSize>> PerformAsync(object? parameter = null) {
        EventBus<ITypeSizesWindowSubscriber>.RaiseEvent<ITypeSizeFormOpenHandler>(h => h?.OpenTsForm<AddItemStateForm>(new TypeSize()));
        //EventBus<ITypeSizesWindowSubscriber>.RaiseEvent<ITypeSizeFormOpenHandler>(h => h?.NotifyToBlock(_context.DataGridViewModel.SelectedItem));
        
        return Task.FromResult<BaseAction<TypeSizesTableWindow, TypeSize>>(this);
    }
}