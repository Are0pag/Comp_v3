using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.Operations.Commands;

public class CommandFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CommandFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public TCommand CreateCommand<TCommand, TParameter>(TParameter parameter)
        where TCommand : BaseCommand<TParameter> 
    {
        // Получаем конструктор команды
        var constructorInfo = typeof(TCommand).GetConstructors()
                                              .OrderByDescending(c => c.GetParameters().Length)
                                              .First();

        ParameterInfo[] parametersInfo = constructorInfo.GetParameters();
        var constructorArgs = new object[parametersInfo.Length];

        // Первый параметр - это TParameter
        constructorArgs[0] = parameter;

        // Остальные параметры получаем из DI
        for (var i = 1; i < parametersInfo.Length; i++) {
            var parameterType = parametersInfo[i].ParameterType;
            constructorArgs[i] = _serviceProvider.GetRequiredService(parameterType);
        }

        return (TCommand)constructorInfo.Invoke(constructorArgs);
    }
}