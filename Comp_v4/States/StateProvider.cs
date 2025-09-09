using Comp.ModelData.TechnicalItems;
using Infrastructure.StateMachine;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public class StateProvider : GenericStateMachine<BaseState, ViewModel>
{
    protected StateProvider(IEnumerable<BaseState> states, BaseState initialState) : base(states, initialState) {
    }
}