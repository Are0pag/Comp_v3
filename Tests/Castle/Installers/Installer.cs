using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tests.Castle.Entities.Ui;

namespace Tests;

public class Installer : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store) {
        container.Register(
            Component.For<FloatingVm>(),
            Component.For<FloatingWindow>()
            );
    }
}