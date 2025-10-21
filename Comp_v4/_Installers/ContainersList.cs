using DI;

namespace Comp_v4.Installers;

public static class ContainersList
{
    private static readonly List<AreopagContainer> _containers = new();

    public static T Get<T>() where T : AreopagContainer, new() {
        var containers = _containers.OfType<T>().ToList();
        var targetContainer = containers switch {
            { Count: 0 } => new T(),
            { Count: 1 } => containers[0],
            _            => throw new Exception($"Uncorrect number of container type {typeof(T).Name}")
        };

        return targetContainer;
    }
}