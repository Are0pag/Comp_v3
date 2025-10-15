using System.Windows.Input;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Templates.Common.Events.Input;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.TableWindows.TypeSizes;

public class ActionOpenTsForm : BaseAction<TypeSizesTableWindow, TypeSize>, IMouseDoubleClickHandler
{
    public ActionOpenTsForm(IDataGridCommandScheduler scheduler, ModuleContext<TypeSizesTableWindow, TypeSize> context, ICommandFactory commandFactory) 
        : base(scheduler, context, commandFactory) {
        EventBus<IGlobalMouseSubscriber>.Subscribe(this);
    }

    public override Task<BaseAction<TypeSizesTableWindow, TypeSize>> PerformAsync(object? parameter = null) {
        if (_context.DataGridViewModel.SelectedItem is not { } selectedItem)
            throw new NullReferenceException();
        EventBus<ITypeSizesWindowSubscriber>.RaiseEvent<ITypeSizeFormOpenHandler>(h => h?.OpenTsForm<EditItemStateForm>(selectedItem));
        return Task.FromResult<BaseAction<TypeSizesTableWindow, TypeSize>>(this);
    }

    public override bool CanPerform() {
        return _context.DataGridViewModel.SelectedItem != null;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }

    public void Dispose() {
        EventBus<IGlobalMouseSubscriber>.Unsubscribe(this);
    }

    void IMouseDoubleClickHandler.OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        PerformAsync();
    }
}