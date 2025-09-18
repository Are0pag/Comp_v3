using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;

namespace Comp_v4.TableWindows.ConditionalDesignation;

public class Installer : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store) {
        IKernel kernel = new DefaultKernel();
        kernel.AddFacility<TypedFactoryFacility>();
        kernel.Register(Component.For<IDummyComponentFactory>()
                                 .AsFactory()
                       );
        
        kernel.Register(
                        Component.For<IDummyComponent>()
                                 .ImplementedBy<Component2>()
                                 .Named("SecondComponent")
                                 .OnCreate((kernel, instance) => {kernel.})
                                 .LifeStyle.Transient,
                        Component.For<IDummyComponentFactory>()
                                 .AsFactory());
        
        var factory = kernel.Resolve<IDummyComponentFactory>();
        var component = factory.Create();
        // use
        factory.Release(component);
        //
        factory.Dispose(); // all non-singleton components will be released as well.
    }
}

public interface IDummyComponentFactory : IDisposable
{
    IDummyComponent Create();
    void Release(params IDummyComponent dummyComponent); // может называться как угодно, главное что void поэтому резолвит
}