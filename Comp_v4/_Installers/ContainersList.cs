using DI;

namespace Comp_v4.Installers;

public static class ContainersList
{
    private static readonly List<AreopagContainer> _containers = new();

    public static T Get<T>() where T : AreopagContainer, new() {
        var containers = _containers.OfType<T>().ToList();
        T? targetContainer;
        switch (containers) {
            case { Count: 0 }:
                targetContainer = new T();
                _containers.Add(targetContainer);
                break;
            case { Count: 1 }:
                targetContainer = containers[0];
                break;
            default:
                throw new Exception($"Uncorrect number of container type {typeof(T).Name}");
        }

        return targetContainer;
    }
}