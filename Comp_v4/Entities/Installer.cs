using Comp.Db.Contracts;

namespace WPF.Templates;

public class Installer
{
    protected readonly IConditionalDesignationRepository _repository;

    public Installer(IConditionalDesignationRepository repository) {
        _repository = repository;
    }
    
    //public async Task LoadData
}