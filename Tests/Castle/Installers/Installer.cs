using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tests.Castle.Entities.Ui;

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