using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Events;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using DI;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.CompCard._Installers;

// регистрируется как синглтон
public class CardComponentManager
{
    protected readonly AreopagContainer _container;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    protected readonly List<Component> _openedComponentsCards = new();

    public CardComponentManager(AreopagContainer container, IWindowOrderLocator windowOrderLocator) {
        _container = container;
        _windowOrderLocator = windowOrderLocator;
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
        _container.SetFactoryMethodFor<IImageOwner>(() => args.Component);
        
        _container.SetFactoryMethodFor<CardComp>(() => {
                       var initialState = _container.Resolve<TInitialState>();
                       var states = new List<BaseStateCardComp>() {
                           _container.Resolve<CreateStateCardComp>(),
                           _container.Resolve<EditStateCardComp>()
                       };
                       return new CardComp(states, initialState);
                   });
        
        var window = _container.BeginScope<CompCardWindow>();
        _windowOrderLocator.RegisterWindow(window);
        window.Closed += (_, __) => {
            _container.ReleaseScope<CompCardWindow>();
            _openedComponentsCards.Remove(args.Component);
        };
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