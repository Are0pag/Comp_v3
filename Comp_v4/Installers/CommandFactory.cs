using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Comp_v4.Operations.Commands.Filtering;
using Infrastructure.Command;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using WPF.Services;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm.Components;
using Tw = Comp_v4.TargetWindow;
using Cd = Comp.ModelData.TechnicalItems.ConditionalDesignation;

namespace Comp_v4.Operations.Commands;

public class DataGridCommandFactory : ICommandFactory
{
    protected readonly AreopagContainer _container;

    public DataGridCommandFactory(AreopagContainer container) {
        _container = container;
    }

    public TCommand CreateCommand<TCommand, TParameter>(TParameter parameter) where TCommand : DeferredCommandBase<TParameter> {
        var command = _container.Resolve<TCommand>(parameter);
        return command;
    }
}

public class WindsorCommandFactory : ICommandFactory
{
    private readonly IKernel _kernel;

    public WindsorCommandFactory(IKernel kernel) {
        _kernel = kernel; // ← IKernel знает о текущем scope
    }

    public TCommand CreateCommand<TCommand, TParameter>(TParameter parameter)
        where TCommand : DeferredCommandBase<TParameter> 
    {
        var isFfff = typeof(TCommand) == typeof(ApplyFilterCommand<Tw, Cd, FiltersVmCd>);
        var deferredCommandBase = _kernel.Resolve<TCommand>(new Arguments {
            { "parameter", parameter }
        });

        return deferredCommandBase;
    }
}


public class CommandFactory : ICommandFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CommandFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public TCommand CreateCommand<TCommand, TParameter>(TParameter parameter)
        where TCommand : DeferredCommandBase<TParameter> 
    {
        var constructorInfos = typeof(TCommand).GetConstructors();
        if (constructorInfos.Length > 1)
            new InvalidOperationException("CommandFactory is not implemented for asserting when command have more then one constructor.").Log(this);
        
        ParameterInfo[] parametersInfo = constructorInfos[0].GetParameters();
        var constructorArgs = new object[parametersInfo.Length];

        // Первый параметр - это TParameter
        constructorArgs[0] = parameter;

        // Остальные параметры получаем из DI
        for (var i = 1; i < parametersInfo.Length; i++) {
            var parameterType = parametersInfo[i].ParameterType;
            constructorArgs[i] = _serviceProvider.GetRequiredService(parameterType);
        }

        return (TCommand)constructorInfos[0].Invoke(constructorArgs);
    }
}