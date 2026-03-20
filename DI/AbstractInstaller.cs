namespace DI;

public abstract class AbstractInstaller
{
    public void Install(AreopagContainer container) {
        InstallBindings(container);
        OnCreated(container);
    }
    
    protected abstract void InstallBindings(AreopagContainer container);

    protected virtual void OnCreated(AreopagContainer container) {
        
    }
}