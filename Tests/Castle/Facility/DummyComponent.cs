namespace Comp_v4.TableWindows.ConditionalDesignation;


public interface IDummyComponentFactory : IDisposable
{
    IDummyComponent Create();
    void Release(params IDummyComponent[] dummyComponent); // может называться как угодно, главное что void поэтому резолвит
}



public interface IDummyComponent : IDisposable { }


public class DummyComponent : IDummyComponent
{
    public DummyComponent() {
        Console.WriteLine("DummyComponent created");
    }

    public void Dispose() { }
}


public class SmiledComponent : IDummyComponent
{
    public void Dispose() { }
}