using System.Reflection;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Db;

public class UpdateItemCommand<T> : DeferredCommandBase<T>
    where T : class, IDbEntity
{
    protected readonly IRepository<T> _repository;
    public UpdateItemCommand(T parameter, IRepository<T> repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        if (await _repository.GetByIdAsync(_parameter.Id) is not T dbInstance)
            throw new InvalidOperationException();
        
        try {
            UpdateProperties(dbInstance, _parameter);
            await _repository.UpdateAsync(dbInstance);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void UpdateProperties(object target, object source) {
        var sourceProperties = source.GetType().GetProperties(
                                                              BindingFlags.Public | BindingFlags.Instance
                                                             );

        foreach (var sourceProp in sourceProperties) {
            if (sourceProp.CanRead) {
                var targetProp = target.GetType().GetProperty(sourceProp.Name);
            
                if (targetProp != null && 
                    targetProp.CanWrite && 
                    targetProp.PropertyType == sourceProp.PropertyType) 
                {
                    var value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                }
            }
        }
    }
}