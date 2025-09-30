namespace WPF.Services;

public static class ContainerExtentions
{
    public static void Instantiate<T1>(this AreopagContainer container) {
        container.Resolve<T1>();
    } 
    
    public static void Instantiate<T1, T2>(this AreopagContainer container) {
        container.Resolve<T1>();
        container.Resolve<T2>();
    } 
    
    public static void Instantiate<T1, T2, T3>(this AreopagContainer container) {
        container.Resolve<T1>();
        container.Resolve<T2>();
        container.Resolve<T3>();
    }
    
    public static void Instantiate<T1, T2, T3, T4>(this AreopagContainer container) {
        container.Resolve<T1>();
        container.Resolve<T2>();
        container.Resolve<T3>();
        container.Resolve<T4>();
    } 
    
    public static void Instantiate<T1, T2, T3, T4, T5>(this AreopagContainer container) {
        container.Resolve<T1>();
        container.Resolve<T2>();
        container.Resolve<T3>();
        container.Resolve<T4>();
        container.Resolve<T5>();
    } 
}