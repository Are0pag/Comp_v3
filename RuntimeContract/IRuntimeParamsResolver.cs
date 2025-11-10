namespace Comp_v4._Installers;

public interface IGlSubscriber : IDisposable {}

public interface IRuntimeParamsResolver<T> : IGlSubscriber
{
    Task ResolveRuntimeParams(IRuntimeParamsContainer<T> container);
}

public interface IRuntimeParamsContainer<T>
{
    T RuntimeParam { get; set; }
}