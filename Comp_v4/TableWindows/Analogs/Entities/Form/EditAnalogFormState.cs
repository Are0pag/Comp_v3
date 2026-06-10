using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class EditAnalogFormState : AddAnalogsFormState
{
    public EditAnalogFormState(IWindowOrderLocator windowOrderLocator, IRepository<Analog> analogRepository, IServiceProvider serviceProvider) 
        : base(windowOrderLocator, analogRepository, serviceProvider) {
    }

    public override async Task Save(AnalogsForm form) {
        await _analogRepository.UpdateAsync(RuntimeParam);
    }
}