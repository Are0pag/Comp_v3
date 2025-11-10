namespace Comp.ModelData;

public interface IPopulatable<T>
{
    T PopulateFrom(T targetValues);
}