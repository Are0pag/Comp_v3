using System.Windows;
using Comp_v4._Installers;
using Utils.EventBus;

namespace Comp_v4;

public class InstanceContainer<T> : IRuntimeParamsContainer<T> where T : Window
{
    protected T _item;
    public T RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<T>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _item;
        }
        set => _item = value;
    }
}