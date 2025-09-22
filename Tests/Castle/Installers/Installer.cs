using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tests.Castle.Entities.Ui;
using WPF.Services;

namespace Tests;

public class Installer : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store) {
        container.Register(
            Component.For<Cat>(),
            Component.For<DogInt>().DependsOn(Property.ForKey<int>()),
            Component.For<FloatingWindow>()
            );
    }
}

public class MyInstaller : AbstractInstaller
{
    protected override void InstallBindings(Container container) {
        /*container.Add<Cat>().AsSingleton();
        container.Add<CatMom>().AsSingleton();

        container.Add<ViewModel>().AsScoped<FloatingWindow>();
        container.Add<FloatingWindow>().AsTransient();*/

        container.Add<ICat>().To<Cat>().AsScoped<ICatMom>();
        container.Add<DogInt>().AsTransient().WithParameters(typeof(int));
        container.Add<ICatMom>().To<CatMom>().AsSingleton();
    }
}