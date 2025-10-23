using Comp.Db.Contracts;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.Counterparties.Table.Vm;

public class DataGridVm : DataGridViewModel<Counterparty>
{
    public DataGridVm(IRepository<Counterparty> repository) : base(repository) {
    }
}