using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;

namespace Comp_v4.TableWindows.ConditionalDesignation;


public class TestInstaller : IWindsorInstaller
{
    public static IKernel Kernel { get; set; }
    
    public void Install(IWindsorContainer container, IConfigurationStore store) {
        Kernel = new DefaultKernel();
        
        Kernel.AddFacility<TypedFactoryFacility>();
        Kernel.Register(
            Component.For<IDummyComponentFactory>()
                     .AsFactory());
        
        Kernel.Register(
            Component.For<IDummyComponent>()
                .ImplementedBy<DummyComponent>()
                .Named("SecondComponent")
                .LifeStyle.Transient);
    }
}
