using Comp_v4.CompCard.Entities.States;
using Comp.ModelData.Comp;
using WPF.Services;

namespace Comp_v4.CompCard.Entities;

// регистрируется пожалуй как синглтон
public class CardComponentManager
{
    protected readonly AreopagContainer _container;
    protected readonly List<Component> _openedComponentsCards = new();
    
    public void OpenWindow<TInitialState>(Component component) 
        where TInitialState : BaseStateCardComp
    {
        if (_openedComponentsCards.Contains(component))
            return;
        _openedComponentsCards.Add(component);
        
        _container.Add<Component>()
                  .AsScoped<CompCardWindow>()
                  .UsingFactoryMethod(() => component);

        _container.Add<EditStateCardComp>().AsScoped<CompCardWindow>();
        _container.Add<CreateStateCardComp>().AsScoped<CompCardWindow>();

        _container.Add<CardComp>().AsScoped<CompCardWindow>()
                  .UsingFactoryMethod(() => {
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
            _openedComponentsCards.Remove(component);
        };
        window.Show();
    }
}