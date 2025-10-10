using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Events;
using Comp_v4.CompCard.Operations.Actions;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Utils.EventBus;
using WPF.Services;

namespace Comp_v4.CompCard.Entities;

// регистрируется как синглтон
public class CardComponentManager
{
    protected readonly AreopagContainer _container;
    protected readonly List<Component> _openedComponentsCards = new();

    public CardComponentManager(AreopagContainer container) {
        _container = container;
    }

    public void OpenWindow<TInitialState>(Args args) 
        where TInitialState : BaseStateCardComp
    {
        if (_openedComponentsCards.Contains(args.Component))
            return;
        _openedComponentsCards.Add(args.Component);


        if (args.Category != null)
            args.Component.Category = args.Category; 
        _container.SetFactoryMethodFor<Component>(() => args.Component);
        
        _container.SetFactoryMethodFor<CardComp>(() => {
                       var initialState = _container.Resolve<TInitialState>();
                       var states = new List<BaseStateCardComp>() {
                           _container.Resolve<CreateStateCardComp>(),
                           _container.Resolve<EditStateCardComp>()
                       };
                       return new CardComp(states, initialState);
                   });
        
        var window = _container.BeginScope<CompCardWindow>();
        window.Closed += (_, __) => {
            _container.ReleaseScope<CompCardWindow>();
            _openedComponentsCards.Remove(args.Component);
        };
        _container.Instantiate<SaveComponentAction, SetUrlAction, SetUrlAlternativeAction, SetFilePathAction>();
        _container.Instantiate<SelectImageAction, OpenImageAction>();
        EventBus<ICompCardSubscriber>.RaiseEvent<ICompCardLoadedHandler>(h => h?.OnCompCardLoaded(args.Component));
        window.Show();
    }

    public class Args
    {
        public readonly Component Component;
        public readonly Category? Category;

        public Args(Component component, Category? category = null) {
            Component = component;
            Category = category;
        }
    }
}