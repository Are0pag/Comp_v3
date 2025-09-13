using Comp_v4.Operations.Commands;
using Comp.ModelData.TechnicalItems;
using WPF.Services.Validation;

namespace Comp_v4.Entities;

public class Validator : ValidatorBase<ConditionalDesignation>
{
    protected override void AddRules() {
        throw new NotImplementedException();
    }
}

public static class CommandsProvider
{
    /*public static FocusCellCommand FocusCellCommand {
        get {
            return App.TestScope.
        }
    } */
}