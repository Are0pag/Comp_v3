namespace WPF.Services;

public abstract class AbstractInstaller
{
    public void Install(Container container) {
        InstallBindings(container);
        OnCreated(container);
    }
    
    protected abstract void InstallBindings(Container container);

    protected virtual void OnCreated(Container container) {
        
    }
}