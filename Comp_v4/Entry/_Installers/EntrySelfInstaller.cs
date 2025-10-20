using Comp_v4.Entry.Entities;
using Comp_v4.Entry.Vm.Actions;
using Comp_v4.Entry.Vm.Buts;
using DI;
using DI.Contracts;

namespace Comp_v4.Entry._Installers;

public class EntrySelfInstaller : ISelfLayerInstaller
{
    public AreopagContainer InstallSelf(AreopagContainer selfContainer) {
        selfContainer.Add<ToolsPanelStateIdle>().AsScoped<EntryWindow>();
        selfContainer.Add<ToolsPanel>()
                     .AsScoped<EntryWindow>()
                     .UsingFactoryMethod(() => {
                          var initialState = selfContainer.Resolve<ToolsPanelStateIdle>();
                          var states = new List<BaseToolsPanelState>() {
                              initialState,
                          };
                          return new ToolsPanel(states, initialState);
                      })
                     .EnforceInstantiateOnBegin();
        
        selfContainer.Add<OpenNomDictAction>().AsScoped<EntryWindow>().EnforceInstantiateOnBegin();
        selfContainer.Add<NomDictButVm>().AsScoped<EntryWindow>();

        selfContainer.Add<EntryWindow>().AsSingleton();
        return selfContainer;
    }
}