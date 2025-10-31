namespace Comp.ModelData;

public interface IPopulable<T>
{
    T PopulateFrom(T targetValues);
}