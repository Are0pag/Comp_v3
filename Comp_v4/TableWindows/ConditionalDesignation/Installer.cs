using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Comp.Db;
using Microsoft.EntityFrameworkCore;

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
                                 .ImplementedBy<DummyComponent>()
                                 .Named("SecondComponent")
                                 .OnCreate((kernel, instance) => {/*kernel.*/})
                                 .LifeStyle.Transient,
                        Component.For<IDummyComponentFactory>()
                                 .AsFactory());
        
        var factory = kernel.Resolve<IDummyComponentFactory>();
        var component = factory.Create();
        // use
        factory.Release(component);
        //
        factory.Dispose(); // all non-singleton components will be released as well.

        
        
        var cont = new WindsorContainer();
        using (cont.BeginScope()) {
            var dummyComponent = cont.Resolve<IDummyComponent>();
        }
        
        // docs/implementing-custom-scope.md 
        // https://github.com/castleproject/Windsor/blob/master/docs/implementing-custom-scope.md
        
        var container2 = new Castle.Windsor.WindsorContainer();
        //container2.Register(Component.For<MyScopedComponent>().LifestyleScoped<MyCustomScopeAccessor>());

        container2.Register(Component.For<CurrentWindow>().LifestyleTransient());
        container2.Register(Component.For<IDummyComponent>().ImplementedBy<SmiledComponent>().LifestyleBoundTo<CurrentWindow>());

        container2.Register(Component.For<ActionsTracker>().LifestyleBoundTo<CurrentWindow>().UsingFactoryMethod(() => new ActionsTracker())); // как сразу создать экземпляр??
        //
        container2.Dispose();

        container.Register(Component.For<AppDbContext>()
                                    .UsingFactoryMethod(kernel => {
                                         var options = new DbContextOptionsBuilder<AppDbContext>()
                                                      .UseSqlite(DbConfig.ConnectionString)
                                                      .Options;

                                         return new AppDbContext(options);
                                     })
                                    .LifestyleScoped() // или .LifestylePerWebRequest() для web
        );
    }
}

public interface IDummyComponentFactory : IDisposable
{
    IDummyComponent Create();
    void Release(params IDummyComponent[] dummyComponent); // может называться как угодно, главное что void поэтому резолвит
}

public interface IDummyComponent : IDisposable
{
    
}

public class SmiledComponent : IDummyComponent
{
    public void Dispose() {
        throw new NotImplementedException();
    }
}

[CastleComponent("SomeDummyComponent", typeof(IDummyComponent), Lifestyle = LifestyleType.Scoped)]
public class DummyComponent : IDummyComponent
{
    public void Dispose() {
        throw new NotImplementedException();
    }
}

public class CurrentWindow
{
    
}

public class ActionsTracker
{
    public ActionsTracker() {
        Console.WriteLine("ActionsTracker is created!");
    }
}