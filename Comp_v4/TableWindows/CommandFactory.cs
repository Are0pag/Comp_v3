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
